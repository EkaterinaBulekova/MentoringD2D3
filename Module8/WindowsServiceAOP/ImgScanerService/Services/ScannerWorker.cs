using System;
using System.IO;
using System.Linq;
using System.Threading;
using ImgScanerService.Interfaces;
using LoggingPostSharpRewriting;
using LoggingService;
using UtilitiesService.ConfigureProps;

namespace ImgScanerService.Services
{
    public class ScannerWorker : IScannerWorker
    {
        private readonly IFileScanner _scanner;
        private readonly IToPdfFilesMerger _filesMerger;
        private readonly IOutputService _outputService;
        private readonly ServiceProps _props;
        private readonly ILogger _logger;

        Thread workingThread;
        ManualResetEvent workStop;
        AutoResetEvent newFile;

        [LoggerAspect]
        public ScannerWorker(IFileScanner scanner, IToPdfFilesMerger filesMerger, IOutputService outputService, ServiceProps props, ILogger logger)
        {
            _scanner = scanner;
            _filesMerger = filesMerger;
            _outputService = outputService;
            _props = props;
            _logger = logger;

            workStop = new ManualResetEvent(false);
            newFile = new AutoResetEvent(false);
            workingThread = new Thread(ScanStart);
        }

        [LoggerAspect]
        public void ScanStart()
        {
            do
            {
                _logger.LogInfo("Scanning... ");
                if (workStop.WaitOne(TimeSpan.Zero))
                    return;

                var files = _scanner.GetFiles();
                var chunk = _scanner.Scan(files);

                if (chunk.ChunkFiles.Any())
                {
                    if (!string.IsNullOrEmpty(chunk.Name))
                    {
                        _filesMerger.Merge(chunk.ChunkFiles, _outputService, $"{chunk.Name ?? "out"}_{DateTime.UtcNow.ToFileTimeUtc()}.pdf");

                        foreach (var file in chunk.ChunkFiles)
                        {
                            _logger.LogInfo(" File.Delete " + file);
                            File.Delete(file);
                        }
                    }
                    else
                    {
                        _logger.LogInfo($"   {chunk.ChunkFiles.Count} files in a chunk. Waiting for barcode.");
                    }
                }
                else
                {
                    _logger.LogInfo("no files in a chunk");
                }


            }
            while (WaitHandle.WaitAny(new WaitHandle[] { workStop, newFile }, _props.ScanInterval) != 0);
        }


        [LoggerAspect]
        public void StartScan()
        {
            workingThread.Start();
        }

        [LoggerAspect]
        public void StopScanning()
        {
            workingThread.Abort();
        }
    }
}
