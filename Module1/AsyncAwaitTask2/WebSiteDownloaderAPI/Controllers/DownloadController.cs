using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebSitesDownloader;
using WebSitesDownloader.Models;

namespace WebSiteDownloaderAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DownloadController : ControllerBase
    {
        private static readonly List<UrlModel> urls = new List<UrlModel>
            {
                new UrlModel{Site = "http://msdn.microsoft.com" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/hh290138.aspx" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/hh290140.aspx" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/dd470362.aspx" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/aa578028.aspx" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/ms404677.aspx" },
                new UrlModel{Site = "http://msdn.microsoft.com/en-us/library/ff730837.aspx" }
            };

        private readonly ILogger<DownloadController> _logger;

        public DownloadController(ILogger<DownloadController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            urls.ForEach(u => Downloader.CreateNew(u));
            return Ok(urls);
        }

        [HttpPut]
        public ActionResult DownloadSite([FromBody]UrlModel model)
        {
            Downloader.CreateNew(model);

            return Ok(model);
        }

        [HttpDelete]
        public ActionResult Cancel([FromBody]UrlModel model)
        {
            return Ok(Downloader.Cancel(model));
        }

        [HttpPost]
        public async Task<UrlModel> StartDownload([FromBody]UrlModel model)
        {
            return await Downloader.StartNew(model);
        }
    }
}
