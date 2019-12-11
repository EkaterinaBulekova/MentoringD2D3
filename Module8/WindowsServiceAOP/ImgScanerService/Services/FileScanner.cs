using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using ImgScanerService.Interfaces;
using ImgScanerService.Models;
using LoggingService;
using LoggingPostSharpRewriting;
using UtilitiesService.Interfaces;
using UtilitiesService.ConfigureProps;
using ZXing;

namespace ImgScanerService.Services
{
    public class FileScanner : IFileScanner
    {
        private readonly IFileSystemHelper _fileSysHelper;
        private readonly string[] _inputDirectories;

        private ILogger _logger;

        [LoggerAspect]
        public FileScanner(ScanProps props, IFileSystemHelper fileSystemHelper, ILogger logger)
        {
            _fileSysHelper = fileSystemHelper;
            _logger = logger;

            _inputDirectories = props.InputLocations;

            fileSystemHelper.CreateDirectoryIfNotExists(_inputDirectories);
            fileSystemHelper.CreateDirectoryIfNotExists(props.OutputLocation);
        }

        [LoggerAspect]
        public ChunkModel Scan(IEnumerable<string> files)
        {
            var chunk = new ChunkModel();

            foreach (var file in files)
            {
                _logger.LogInfo("   file:" + file);

                if (_fileSysHelper.TryOpen(file, 3))
                {
                    string barcodeText = GetBarcodeIfExists(file);
                    if (!string.IsNullOrEmpty(barcodeText))
                    {
                        File.Delete(file);
                        chunk.Name = barcodeText;

                        break;
                    }

                    chunk.ChunkFiles.Add(file);
                }
            }
            return chunk;
        }

        [LoggerAspect]
        public string GetBarcodeIfExists(string file)
        {
            var reader = new BarcodeReader { AutoRotate = true };
            using (var bmp = (Bitmap) Bitmap.FromFile(file))
            {
                var result = reader.Decode(bmp);
                bmp.Dispose();

                return result != null ? result.Text : null;
            }
        }

        [LoggerAspect]
        public IEnumerable<string> GetFiles(string[] inputDirectories = null)
        {
            const string IMG_FILE_PATTERN = "Image_*.*";
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg", ".bmp" };
            var files = new List<string>();

            foreach (var folder in inputDirectories ?? _inputDirectories)
            {
                _logger.LogInfo("GetFiles from: " + folder);
                var filesToAdd = _fileSysHelper.GetFiles(folder, IMG_FILE_PATTERN, allowedExtensions);
                files.AddRange(filesToAdd);
            }
            return files.OrderBy(Path.GetFileName).ToList();
        }
    }
}
