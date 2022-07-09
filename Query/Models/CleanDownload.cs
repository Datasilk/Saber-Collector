using System;
using System.Collections.Generic;

namespace Query.Models
{
    public class CleanDownload
    {
        public int totalArticles { get; set; }
        public int totalDownloads { get; set; }
        public List<Article> articles { get; set; }
        public List<DownloadQueue> downloads { get; set; }
    }
}
