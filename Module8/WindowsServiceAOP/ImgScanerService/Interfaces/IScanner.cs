using System.Collections.Generic;
using ImgScanerService.Models;

namespace ImgScanerService.Interfaces
{
    public interface IFileScanner
    {
        ChunkModel Scan(IEnumerable<string> files);
        string GetBarcodeIfExists(string file);
        IEnumerable<string> GetFiles(string[] inputFolders = null);
    }
}
