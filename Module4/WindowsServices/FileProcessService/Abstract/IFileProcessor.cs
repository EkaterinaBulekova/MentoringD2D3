using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace FileProcessService
{
    public interface IFileProcessor
    {
        string TargetDirectory { set; }

        void Add(FileInfo file);

        void New();
    }
}
