using System;
using System.IO;

namespace FileProcessService
{
    public interface IFileProcessQueue
    {
        void Start();

        void EnqueueItem(FileInfo file);

        void Shutdown(bool waitForWorker);
    }
}