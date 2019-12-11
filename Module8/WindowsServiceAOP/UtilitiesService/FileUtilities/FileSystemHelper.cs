using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using LoggingPostSharpRewriting;
using UtilitiesService.Interfaces;

namespace UtilitiesService.Files
{
    public class FileSystemHelper : IFileSystemHelper
    {
        [LoggerAspect]
        public void CreateDirectoryIfNotExists(params string[] directories)
        {
            foreach (var dir in directories)
            {
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
            }
        }

        [LoggerAspect]
        public bool TryOpen(string fullPath, int tryCount, int sleepTime = 3000)
        {
            for (var i = 0; i < tryCount; i++)
            {
                try
                {
                    File.Open(fullPath, FileMode.Open, FileAccess.Read, FileShare.None).Close();
                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(sleepTime);
                }
            }

            return false;
        }

        [LoggerAspect]
        public IEnumerable<string> GetFiles(string folder, string filePattern = "*.*", string[] allowedExtensions = null)
        {
            var filesToAdd = Directory.GetFiles(folder, filePattern);

            if (allowedExtensions != null && allowedExtensions.Length > 0)
            {
                filesToAdd = filesToAdd.Where(file => allowedExtensions.Any(file.ToLower().EndsWith)).ToArray();
            }
            return filesToAdd.OrderBy(Path.GetFileName).ToList();
        }
    }
}
