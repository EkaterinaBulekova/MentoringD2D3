using System;
using System.Collections.Generic;
using System.Text;

namespace WebSitesDownloader.Models
{
    public class UrlModel
    {
        public string Site { get; set; }
        public Guid ID { get; set; }
        public string Content { get; set; }
    }
}
