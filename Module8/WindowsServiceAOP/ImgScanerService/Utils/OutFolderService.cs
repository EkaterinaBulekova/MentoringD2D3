using System.IO;
using ImgScanerService.Interfaces;
using LoggingService;
using LoggingPostSharpRewriting;
using UtilitiesService.ConfigureProps;

namespace ImgScanerService.Utils
{
    public class OutFolderService : IOutputService
    {
        private string _outputLocation;
        private ILogger _logger;

        [LoggerAspect]
        public OutFolderService(ScanProps props, ILogger logger)
        {
            _logger = logger;
            this._outputLocation = props.OutputLocation;
        }

        [LoggerAspect]
        public void SaveToOut(string fileName)
        {
            File.Move(fileName, Path.Combine(_outputLocation, Path.GetFileName(fileName)));
        }

        [LoggerAspect]
        public void SaveToOut(Stream stream, string fileName)
        {
            fileName = Path.Combine(_outputLocation, fileName);
            _logger.LogInfo(" Save file: " + fileName);
            using (var fileStream = File.Create(fileName))
            {
                stream.CopyTo(fileStream);
                fileStream.Close();
            }
        }
    }
}
