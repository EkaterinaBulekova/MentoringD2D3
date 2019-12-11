using System.Collections.Generic;

namespace UtilitiesService.Interfaces
{
    public interface IFileSystemHelper
    {
        void CreateDirectoryIfNotExists(params string[] folders);
        bool TryOpen(string fullPath, int tryCount, int sleepTime = 3000);
        IEnumerable<string> GetFiles(string folder, string filePattern = "*.*", string[] allowedExtensions = null);
    }
}
