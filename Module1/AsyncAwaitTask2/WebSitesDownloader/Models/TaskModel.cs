using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WebSitesDownloader.Models
{
    public class TaskModel
    {
        public string Site { get; set; }
        public Guid ID { get; set; }
        public WebClient client { get; set; }
        public TaskModel(UrlModel model)
        {
            Site = model.Site;
            ID = model.ID;
        }
    }
}
