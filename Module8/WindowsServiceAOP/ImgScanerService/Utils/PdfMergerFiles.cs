using System.Collections.Generic;
using System.IO;
using ImgScanerService.Interfaces;
using LoggingService;
using LoggingPostSharpRewriting;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;


namespace ImgScanerService.Utils
{
    public class PdfMergerFiles : IToPdfFilesMerger 
    {
        private ILogger _logger;

        [LoggerAspect]
        public PdfMergerFiles(ILogger logger)
        {
            _logger = logger;
        }

        [LoggerAspect]
        public void Merge(IList<string> filesToMerge, IOutputService storageService, string path)
        {
            var document = new Document();
            document.AddSection();

            foreach (var file in filesToMerge)
            {
                Merge(file, document);
            }
            PushChanges(document, storageService, path);
        }

        [LoggerAspect]
        private void Merge(string inputFile, Document document)
        {
            _logger.LogInfo(" Handle: " + inputFile);
            var section = document.Sections[0];

            var img = section.AddImage(inputFile);
            img.RelativeHorizontal = RelativeHorizontal.Page;
            img.RelativeVertical = RelativeVertical.Page;

            img.Top = 0;
            img.Left = 0;

            img.Height = document.DefaultPageSetup.PageHeight;
            img.Width = document.DefaultPageSetup.PageWidth;

            section.AddPageBreak();
        }


        [LoggerAspect]
        private void PushChanges(Document  document, IOutputService storageService, string path)
        {
            _logger.LogInfo(" PushChanges: " + path);
            var render = new PdfDocumentRenderer { Document = document };
            render.RenderDocument();

            using (var ms = new MemoryStream())
            {
                render.Save(ms, false);
                storageService.SaveToOut(ms, path);
                ms.Close();
            }
        }
    }
}
