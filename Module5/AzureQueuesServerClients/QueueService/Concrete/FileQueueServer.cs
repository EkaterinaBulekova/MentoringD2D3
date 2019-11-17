using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using Microsoft.Extensions.Logging;
using CofigurationService;
using System.Collections.Generic;
using System.Linq;

namespace FileQueueService
{
    public class FileQueueServer : IQueueService
    {
        private readonly IConfigurator _config;
        private readonly ILogger<FileQueueServer> _logger;
        private readonly ManagementClient _manager;
        private readonly CloudBlobClient _blobClient;
        private QueueClient queueClient;
        private TopicClient topicClient;
        private Dictionary<string, ServiceStatus> clientsStatuses = new Dictionary<string, ServiceStatus>();

        public FileQueueServer(ILogger<FileQueueServer> logger, IConfigurator config)
        {
            _config = config;
            _logger = logger;
            _manager = new ManagementClient(_config.ServiceBusConnectionString);
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse(_config.StorageAccountConnectionString);
            _blobClient = cloudStorageAccount.CreateCloudBlobClient();
        }

        public async Task Start(CancellationToken token)
        {
            await CreateQueue();
            await CreateTopic();
            RegisterOnMessageHandlerAndReceiveMessages();
        }

        public async Task Status(ServiceStatus status)
        {
            var data = status.ToString();
            Message message = new Message(Encoding.UTF8.GetBytes(data))
            {
                MessageId = Guid.NewGuid().ToString(),
            };

            try
            {
                await topicClient.SendAsync(message);
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
            if (topicClient != null)
                try
                {
                    await topicClient.CloseAsync();
                    _logger.LogInformation($"Closed {_config.TopicName}");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
        }

        public async Task CleanUp()
        {
            try
            {
                var deleteDate = DateTime.Now - TimeSpan.FromHours(_config.HoursAttachmentLive);
                CloudBlobContainer container = _blobClient.GetContainerReference("attachments");
                if (await container.ExistsAsync())
                {
                    BlobContinuationToken blobContinuationToken = null;
                    var blobList = await container.ListBlobsSegmentedAsync(blobContinuationToken);
                    var cloudBlobList = blobList.Results.Select(blb => blb as ICloudBlob).Where(b => b.Properties.LastModified <= deleteDate);
                    foreach (var item in cloudBlobList)
                    {
                        await item.DeleteIfExistsAsync();
                    }
                }
                else
                {
                    _logger.LogError("No Blob Container Available");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }

        private async Task CreateQueue()
        {
            var storageConfiguration = new AzureStorageAttachmentConfiguration(_config.StorageAccountConnectionString, messageMaxSizeReachedCriteria: (message) => message.Body.Length > 200);

            QueueDescription description = new QueueDescription(_config.QueueName)
            {
                EnablePartitioning = true,
                LockDuration = new TimeSpan(0, 0, 30),
                MaxDeliveryCount = 2000,
                DefaultMessageTimeToLive = new TimeSpan(14, 0, 0, 0),
            };
            if (!await _manager.QueueExistsAsync(_config.QueueName))
            {
                await _manager.CreateQueueAsync(description);
                _logger.LogInformation($"Created {_config.QueueName }");
                await CreateContainer();
            }

            queueClient = new QueueClient(_config.ServiceBusConnectionString, _config.QueueName, ReceiveMode.PeekLock);
            queueClient.RegisterAzureStorageAttachmentPlugin(storageConfiguration);
        }

        private async Task CreateContainer()
        {
            CloudBlobContainer container = _blobClient.GetContainerReference("attachments");
            try
            {
                if (await container.CreateIfNotExistsAsync())
                    _logger.LogInformation($"Created {container.Name}");
            }
            catch (StorageException e)
            {
                _logger.LogError($"HTTP error code {e.RequestInformation.HttpStatusCode}: {e.RequestInformation.ErrorCode}");
                _logger.LogError(e.Message);
            }
        }

        private async Task CreateTopic()
        {
            _logger.LogInformation($"Created {_config.TopicName} ");

            TopicDescription description = new TopicDescription(_config.TopicName);
            if (!await _manager.TopicExistsAsync(_config.TopicName))
            {
                await _manager.CreateTopicAsync(description);
            }

            topicClient = new TopicClient(_config.ServiceBusConnectionString, _config.TopicName);
        }

        private void RegisterOnMessageHandlerAndReceiveMessages()
        {
            var messageHandlerOptions = new MessageHandlerOptions(ExceptionReceivedHandler)
            {
                MaxConcurrentCalls = 1,
                AutoComplete = false
            };

            queueClient.RegisterMessageHandler(ProcessMessagesAsync, messageHandlerOptions);
        }

        private async Task ProcessMessagesAsync(Message message, CancellationToken token)
        {
            _logger.LogInformation($"Received message: {message.MessageId} from {message.CorrelationId}");
            await queueClient.CompleteAsync(message.SystemProperties.LockToken);
            if (message.ContentType == "file")
                await File.WriteAllBytesAsync(GetNewFilePath(message), message.Body, token);
            if (message.ContentType == "status")
            {
                var result = Encoding.UTF8.GetString(message.Body);
                _logger.LogInformation($"Received client status message: {result}");
                var status = (result == ServiceStatus.Wait.ToString()) ? ServiceStatus.Wait
                             : (result == ServiceStatus.Work.ToString()) ? ServiceStatus.Work
                             : ServiceStatus.Stop;
                if (clientsStatuses.ContainsKey(message.CorrelationId))
                    clientsStatuses[message.CorrelationId] = status;
                else
                    clientsStatuses.Add(message.CorrelationId, status);
            }
        }

        private string GetNewFilePath(Message message)
        {
            var dir = Path.Combine(_config.TargetDirectory, message.CorrelationId);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            return Path.Combine(dir, message.Label);
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
    }
}