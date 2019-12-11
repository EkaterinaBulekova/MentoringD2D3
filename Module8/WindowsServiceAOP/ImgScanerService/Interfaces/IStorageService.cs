using System;
using System.Collections.Generic;
using System.IO;


namespace ImgScanerService.Interfaces
{
    public interface IOutputService
    {
        void SaveToOut(string fileName);
        void SaveToOut(Stream fileStream, string path);
    }
}
