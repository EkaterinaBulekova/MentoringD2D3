using CofigurationService;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace FileProcessService
{
    public class ProcessService<T> : IFileProcessService where T : IFileProcessor, new()
    {
        private const string
            create = "create",
            start = "start",
            enqueue = "enqueue",
            shutdown = "shutdovn";
        private readonly Func<string, Type, string, string> CannotText = (action, type, dir) => $"Cannot {action} {type.Name} {dir}";
        private readonly IConfigurator _config;
        private readonly Dictionary<string, IFileProcessQueue> _queues = new Dictionary<string, IFileProcessQueue>();
        private readonly ILogger<ProcessService<T>> _logger;

        public ProcessService(IConfigurator config, ILogger<ProcessService<T>> logger)
        {
            try
            {
                _config = config;
                _logger = logger;
            }
            catch (System.Exception ex)
            {
                var errorText = CannotText(create, typeof(ProcessService<T>), "");
                _logger.LogError(errorText);
                throw new CannotCreateFileProcessServiceException(errorText, ex);
            }
        }

        public void ShutdownAll()
        {
            foreach (var q in _queues)
            {
                try
                {
                    q.Value.Shutdown(true);
                }
                catch (System.Exception ex)
                {
                    var errorText = CannotText(shutdown, typeof(FileProcessQueue), q.Key);
                    _logger.LogError(errorText);
                    throw new CannotCreateFileProcessQueueException(errorText, ex);
                }
            }
        }

        public void CreateQueues(string[] directories)
        {
            foreach (var dir in directories)
            {
                try
                {
                    _queues.Add(dir, new FileProcessQueue(_config, new T()));
                }
                catch (System.Exception ex)
                {
                    var errorText = CannotText(create, typeof(FileProcessQueue), dir);
                    _logger.LogError(errorText);
                    throw new CannotCreateFileProcessQueueException(errorText, ex);
                }
            }
        }

        public void StartQueues()
        {
            foreach (var q in _queues)
            {
                try
                {
                    q.Value.Start();
                }
                catch (System.Exception ex)
                {
                    var errorText = CannotText(start, typeof(FileProcessQueue), q.Key);
                    _logger.LogError(errorText);
                    throw new CannotStartFileProcessQueueException(errorText, ex);
                }
            }
        }

        public void Enqueue(FileInfo file)
        {
            try
            {
                _queues[file.DirectoryName].EnqueueItem(file);
            }
            catch (System.Exception ex)
            {
                var errorText = CannotText(enqueue, typeof(FileProcessQueue), file.DirectoryName);
                _logger.LogError(errorText);
                throw new CannotEnqueueFileProcessQueueException(errorText, ex);
            }
        }
    }
}
