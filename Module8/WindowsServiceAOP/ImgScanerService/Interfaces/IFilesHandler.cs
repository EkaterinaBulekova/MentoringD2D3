using System.Collections.Generic;


namespace ImgScanerService.Interfaces
{
    public interface IFilesHandler
    {
        void Merge(IList<string> filesToMerge, IOutputService storageService, string path);
    }
}
