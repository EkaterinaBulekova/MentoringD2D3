using System.IO;

namespace FileProcessService
{
    public interface IFileProcessService
    {
        void CreateQueues(string[] directories);

        void StartQueues();

        void Enqueue(FileInfo file);

        void ShutdownAll();
    }
}
