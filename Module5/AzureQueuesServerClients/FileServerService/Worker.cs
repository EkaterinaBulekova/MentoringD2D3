using System;
using System.Threading;
using System.Threading.Tasks;
using CofigurationService;
using FileQueueService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileServerService
{
    public class Worker : BackgroundService
    {
        private const string
                            start = "start",
                            stop = "stop",
                            run = "run",
                            dispose = "dispose";
        private readonly int _timeout;
        private readonly int _hoursAttachmentsLive;
        private readonly ILogger<Worker> _logger;
        private readonly Func<string, string> logWorkerText = (processName) => $"Worker {processName}";
        private readonly Func<object, string> logCreateText = (obj) => $"Created {obj.GetType().Name}";
        private readonly Func<Exception, string> logExceptionText = (ex) => $"Exception - {ex.Message} - {ex.InnerException?.Message}";
        private readonly IQueueService _service;
        private Timer _timer;

        public Worker(ILogger<Worker> logger, IConfigurator config, IQueueService service)
        {
            try
            {
                _logger = logger;
                _logger.LogInformation(logCreateText(logger));
                if (config == null) 
                    logger.LogError("Config isn't valid - null");
                else
                {
                    _timeout = config.WaitTimeout;
                    _hoursAttachmentsLive = config.HoursAttachmentLive;
                }
                _logger.LogInformation(logCreateText(config));
                _service = service;
                _logger.LogInformation(logCreateText(service));
            }
            catch (CannotCreateConfiguratorException ex)
            {
                _logger.LogError(logExceptionText(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
            }
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromHours(_hoursAttachmentsLive));

                await _service.Start(cancellationToken);
                _logger.LogInformation(logWorkerText(start));            
                await ExecuteAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
                await StopAsync(cancellationToken);
            }
        }

        private void DoWork(object state)
        {
            _service.CleanUp();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(logWorkerText(run));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await _service.Status(ServiceStatus.Work);
                    await Task.Delay(_timeout, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            await _service.Status(ServiceStatus.Stop);
            _timer?.Change(Timeout.Infinite, 0);
            await _service.Shutdown();

            _logger.LogInformation(logWorkerText(stop));
        }

        public override void Dispose()
        {
            _logger.LogInformation(logWorkerText(dispose));
            _timer?.Dispose();
            base.Dispose();
        }
    }
}
