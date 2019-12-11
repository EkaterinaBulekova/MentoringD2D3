using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImgScanerService.Interfaces
{
    public interface IScannerWorker
    {
        void StartScan();
        void StopScanning();
    }
}
