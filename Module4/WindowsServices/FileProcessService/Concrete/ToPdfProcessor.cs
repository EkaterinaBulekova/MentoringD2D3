using CofigurationService;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.IO;
using System.Threading;

namespace FileProcessService
{
    public class ToPdfProcessor : IFileProcessor
    {
        private string _targetDirectory;
        private string _tempDirectory;
        private Document _document;
        private Section _section;
        private PdfDocumentRenderer _render;

        public string TargetDirectory { set => _targetDirectory = value; }

        public ToPdfProcessor()
        {
            _document = new Document();
            _section = _document.AddSection();
        }

        public void Add(FileInfo file)
        {
            if (!Directory.Exists(_tempDirectory)) CreateTempDirectory();
            var outFile = MoveTo(file, _tempDirectory);
            if (string.IsNullOrWhiteSpace(outFile)) return;
            AddImageToSection(outFile);
        }

        public void New()
        {
            RenderDocument();
            if (_render.PageCount > 0)
            {
                SaveDocument();
                DeleteTempDirectory();
                CreateNewDocument();
            }
        }      
        private void AddImageToSection(string outFile)
        {
            var img = _section.AddImage(outFile);
            img.Height = _document.DefaultPageSetup.PageHeight;
            img.Width = _document.DefaultPageSetup.PageWidth;
        }        
        
        private string MoveTo(FileInfo file, string path)
        {
            var fileName = file.FullName;
            var outFileName = Path.Combine(path, file.Name);

            if (TryOpen(fileName, 3))
            {
                File.Move(fileName, outFileName);
                return outFileName;
            }
            return null;
        }

        private bool TryOpen(string fileName, int tryCount)
        {
            for (int i = 0; i < tryCount; i++)
            {
                try
                {
                    var file = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.None);
                    file.Close();
                    return true;
                }
                catch (IOException)
                {
                    Thread.Sleep(5000);
                }
            }
            return false;
        }
        
        private void SaveDocument()
        {
            if (!Directory.Exists(_targetDirectory)) 
                Directory.CreateDirectory(_targetDirectory);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _render.Save(GetNewFilePath());
        }

        private void CreateNewDocument()
        {
            _document = new Document();
            _section = _document.AddSection();
        }

        private void CreateTempDirectory()
        {
            _tempDirectory = Path.Combine(_targetDirectory, Guid.NewGuid().ToString());
            Directory.CreateDirectory(_tempDirectory);
        }

        private void RenderDocument()
        {
            _render = new PdfDocumentRenderer
            {
                Document = _document
            };
            _render.RenderDocument();
        }

        private string GetNewFilePath()
        {
            return Path.Combine(_targetDirectory, $"result{Guid.NewGuid()}.pdf");
        }

        private void DeleteTempDirectory()
        {
            if (Directory.Exists(_tempDirectory))
                Directory.Delete(_tempDirectory, true);
        }
    }
}
