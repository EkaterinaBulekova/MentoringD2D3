using System.Threading;
using System.Collections.Generic;
using System.IO;
using CofigurationService;

namespace FileProcessService
{
    public class FileProcessQueue : IFileProcessQueue
    {
        private const int InitialIndex = -1;
        private const int Increment = 1;
        private Thread _worker;
        private readonly Queue<FileInfo> _itemQ = new Queue<FileInfo>();
        private readonly object _locker = new object();
        private readonly IConfigurator _config;
        private readonly IFileProcessor _processor;

        public FileProcessQueue(IConfigurator config, IFileProcessor processor)
        {
            try
            {
                _config = config;
                _processor = processor;
                _processor.TargetDirectory = _config.Directories.TargetDirectory;
            }
            catch (System.Exception ex)
            {
                throw new CannotCreateFileProcessQueueException("Cannot create FileProcessQueue", ex);
            }
        }

        public void Start()
        {
            try
            {
                (_worker = new Thread(Consume)).Start();
            }
            catch (System.Exception ex)
            {
                throw new CannotStartFileProcessQueueException("Cannot start FileProcessQueue", ex);
            }
        }

        public void Shutdown(bool waitForWorker)
        {
            _itemQ.Enqueue(null);
            if (waitForWorker)
                _worker.Join();
        }

        public void EnqueueItem(FileInfo file)
        {
            lock (_locker)
            {
                _itemQ.Enqueue(file);
                Monitor.Pulse(_locker);
            }
        }

        private void Consume()
        {
            var index = InitialIndex;

            while (true)
            {
                FileInfo newFile;
                lock (_locker)
                {
                    while (_itemQ.Count == 0)
                    {
                        if (index == InitialIndex)
                            Monitor.Wait(_locker);
                        else
                            Monitor.Wait(_locker, _config.ProcessRule.Timeout);
                        if (_itemQ.Count == 0)
                        {
                            _processor.New();
                            index = InitialIndex;
                            Monitor.Wait(_locker);
                        }
                    }
                    while (!_itemQ.TryDequeue(out newFile)) ;
                }
                if (newFile == null) return;
                index = Process(index, newFile);
            }
        }

        private int Process(int index, FileInfo newFile)
        {
            var newIndex = _config.ProcessRule.GetIndex(newFile.Name);
            if (newIndex - index == Increment)
            {
                _processor.Add(newFile);
            }
            else
            {
                _processor.New();
                _processor.Add(newFile);
            }
            index = newIndex;
            return index;
        }
    }
}