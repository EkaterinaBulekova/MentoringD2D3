using System;
using System.Collections.Concurrent;
using System.Net;
using System.Threading.Tasks;
using WebSitesDownloader.Models;

namespace WebSitesDownloader
{
    public class Downloader
    {
        private static ConcurrentDictionary<Guid, TaskModel> DownloaderManager
            = new ConcurrentDictionary<Guid, TaskModel>();

        public static void CreateNew(UrlModel model)
        {
            model.ID = Guid.NewGuid();
            var taskmodel = new TaskModel(model);
            taskmodel.client = new WebClient();
            DownloaderManager.TryAdd(model.ID, taskmodel);
        }

        public static async Task<UrlModel> StartNew(UrlModel model)
        {
            TaskModel taskmodel;
            DownloaderManager.TryRemove(model.ID, out taskmodel);
            var client = taskmodel.client;
            var uri = new Uri(taskmodel.Site);
            taskmodel = null;
            //var newModel = new UrlModel { ID = model.ID, Site = taskmodel.Site };
            model.Content = await client.DownloadStringTaskAsync(uri);
            return model;
        }

        public static UrlModel Cancel(UrlModel model)
        {
            TaskModel taskmodel;
            DownloaderManager.TryGetValue(model.ID, out taskmodel);
            if (taskmodel?.client != null)
                taskmodel.client.CancelAsync();
            model.Content = "Canceled";

            return model;
        }

    }
}
