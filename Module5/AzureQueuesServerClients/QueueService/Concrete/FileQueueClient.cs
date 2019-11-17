using System;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using CofigurationService;

namespace FileQueueService
{
    public class FileQueueClient: IQueueService
    {
        private readonly IConfigurator _config;
        private readonly ILogger<FileQueueClient> _logger;
        private readonly ManagementClient _manager;
        private readonly CloudBlobClient _blobClient;
        private SubscriptionClient subscriptionClient;
        private QueueClient queueClient;
        private ServiceStatus serverStatus = ServiceStatus.Stop;

        public FileQueueClient(ILogger<FileQueueClient> logger, IConfigurator config)
        {
            _config = config;
            _logger = logger;
            _manager = new ManagementClient(_config.ServiceBusConnectionString);
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_config.StorageAccountConnectionString);
            _blobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task Start(CancellationToken stoppingToken)
        {
            if (await TryConnectQueue())
                if (await TrySubscribeTopic())
                {
                    RegisterOnMessageHandlerAndReceiveMessages();
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            if (serverStatus == ServiceStatus.Work)
                                await SendFilesAsync(stoppingToken);
                            await Task.Delay(_config.WaitTimeout, stoppingToken);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex.Message);
                            throw new SendFileToMessageException("Cannot send files to queue.", ex);
                        }
                    }
                    _logger.LogInformation("Start client");
                }
        }

        private async Task<bool> TrySubscribeTopic()
        {
            var topicExists = await _manager.TopicExistsAsync(_config.TopicName);

            if (topicExists)
            {
                if (!await _manager.SubscriptionExistsAsync(_config.TopicName, _config.CilentName))
                {
                    await _manager.CreateSubscriptionAsync(_config.TopicName, _config.CilentName);
                }
                subscriptionClient = new SubscriptionClient(_config.ServiceBusConnectionString, _config.TopicName, _config.CilentName, ReceiveMode.PeekLock);
                return true;
            }
            return false;
        }

        private async Task<bool> TryConnectQueue()
        {
            var storageConfiguration = new AzureStorageAttachmentConfiguration(_config.StorageAccountConnectionString, messageMaxSizeReachedCriteria: (message)=>message.Body.Length > 200);
            var ifQueueExists = await _manager.QueueExistsAsync(_config.QueueName);
            var ifContainerExists = await(_blobClient.GetContainerReference("attachments")).ExistsAsync();

            if (ifQueueExists && ifContainerExists)
            {
                _logger.LogInformation($"{_config.QueueName } exists");
                queueClient = new QueueClient(_config.ServiceBusConnectionString, _config.QueueName, ReceiveMode.PeekLock);
                queueClient.RegisterAzureStorageAttachmentPlugin(storageConfiguration);
                return true;
            }
            return false;
        }

        public async Task Status(ServiceStatus status)
        {
            var data = status.ToString();
            Message message = new Message(Encoding.UTF8.GetBytes(data)) 
            {
                CorrelationId = _config.CilentName,
                MessageId = Guid.NewGuid().ToString(),
                ContentType = "status"
            };

            try
            {
                await queueClient.SendAsync(message);
                _logger.LogInformation($"Send status - {data}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        public async Task Shutdown()
        {
            if (queueClient != null)
                try
                {
                    await queueClient.CloseAsync();
                    _logger.LogInformation($"Closed {_config.QueueName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            if (subscriptionClient != null)
                try
                {
                    await subscriptionClient.CloseAsync();
                    if (await _manager.SubscriptionExistsAsync(_config.TopicName, _config.CilentName))
                    {
                        await _manager.DeleteSubscriptionAsync(_config.TopicName, _config.CilentName);
                    }
                    _logger.LogInformation($"Closed {_config.TopicName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            subscriptionClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            var result = Encoding.UTF8.GetString(message.Body);
            _logger.LogInformation($"Received server status message: {result}");
            serverStatus = (result == ServiceStatus.Work.ToString()) ? ServiceStatus.Work : ServiceStatus.Stop;

            await subscriptionClient.CompleteAsync(message.SystemProperties.LockToken);
        }

        private async Task SendFilesAsync(CancellationToken token)
        {
            if (Directory.Exists(_config.TargetDirectory))
            {
                foreach (var file in Directory.GetFiles(_config.TargetDirectory))
                {
                    if (token.IsCancellationRequested) return;
                    var messageBytes = await File.ReadAllBytesAsync(file);
                    var clientname = WindowsIdentity.GetCurrent().Name;
                    var message = new Message(messageBytes)
                    {
                        CorrelationId = _config.CilentName,
                        MessageId = Guid.NewGuid().ToString(),
                        Label = Path.GetFileName(file),
                        ContentType = "file"
                    };

                    await queueClient.SendAsync(message);
                    _logger.LogInformation($"Sent message: {message.MessageId}");
                    var sentDirectory = Path.Combine(_config.TargetDirectory, "Sent");
                    if (!Directory.Exists(sentDirectory)) Directory.CreateDirectory(sentDirectory);
                    var outfile = Path.Combine(sentDirectory, Path.GetFileName(file));
                    File.Move(file, outfile);
                }
            }
        }

        private Task ExceptionReceivedHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
        {
            _logger.LogError($"Message handler encountered an exception {exceptionReceivedEventArgs.Exception}.");
            var context = exceptionReceivedEventArgs.ExceptionReceivedContext;
            _logger.LogError("Exception context for troubleshooting:");
            _logger.LogError($"- Endpoint: {context.Endpoint}");
            _logger.LogError($"- Entity Path: {context.EntityPath}");
            _logger.LogError($"- Executing Action: {context.Action}");
            return Task.CompletedTask;
        }

        public Task CleanUp()
        {
            throw new NotImplementedException();
        }
    }
}
