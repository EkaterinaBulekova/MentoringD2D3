using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CofigurationService;
using FileProcessService;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FileWorkerService
{
    public class Worker : BackgroundService
    {
        private const string
                            start = "start",
                            stop = "stop",
                            run = "run",
                            dispose = "dispose";
        private readonly Directories _directories;
        private readonly FileProcessRule _processRule;
        private readonly ILogger<Worker> _logger;
        private readonly Func<string, string> logWorkerText = (processName) => $"Worker {processName}";
        private readonly Func<object, string> logCreateText = (obj) => $"Created {obj.GetType().Name}";
        private readonly Func<Exception, string> logExceptionText = (ex) => $"Exception - {ex.Message} - {ex.InnerException?.Message}";

        private readonly IFileProcessService _processService;

        public Worker(ILogger<Worker> logger, IConfigurator config, IFileProcessService processService)
        {
            try
            {
                _logger = logger;
                _logger.LogDebug(logCreateText(logger));
                if (config == null) logger.LogError("Config isn't valid - null");
                else
                {
                    _directories = config.Directories;
                    _processRule = config.ProcessRule;
                }
                _logger.LogDebug(logCreateText(config));
                _processService = processService;
                _processService.CreateQueues(_directories.WatchedDirectories);
                _logger.LogDebug(logCreateText(processService));
            }
            catch (CannotCreateConfiguratorException ex)
            {
                _logger.LogError(logExceptionText(ex));
            }
            catch (CannotCreateFileProcessServiceException ex)
            {
                _logger.LogError(logExceptionText(ex));
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
            }
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation(logWorkerText(start));
                _processService.StartQueues();
            }
            catch (CannotStartFileProcessQueueException ex)
            {
                _logger.LogError(logExceptionText(ex));
                StopAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(logExceptionText(ex));
                StopAsync(cancellationToken);
            }

            return base.StartAsync(cancellationToken);
        }

        private void WorkProcedure()
        {
            try
            {
                foreach (var dir in _directories.WatchedDirectories)
                {
                    _logger.LogDebug($"Check directory - {dir}");
                    var dirInfo = new DirectoryInfo(dir);
                    if (dirInfo.Exists)
                    {
                        var files = _processRule.GetAllMatchRule(dirInfo.GetFiles());
                        if (files.Any())
                            files.ForEach(_ => _processService.Enqueue(_));
                    }
                }
            }
            catch (Exception ex)
            {
                throw new WorkProcedureException(ex);
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(logWorkerText(run));
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation($"{logWorkerText(start)} WorkProcedure");
                    WorkProcedure();
                    await Task.Delay(_directories.WatchTimeout, stoppingToken);
                }
                catch (WorkProcedureException ex)
                {
                    _logger.LogError(ex.Message);
                    await StopAsync(stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                    await StopAsync(stoppingToken);
                }
            }
        }

        public override Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation(logWorkerText(stop));
            _processService.ShutdownAll();
            return base.StopAsync(cancellationToken);
        }

        public override void Dispose()
        {
            _logger.LogInformation(logWorkerText(dispose));
            base.Dispose();
        }
    }
}

