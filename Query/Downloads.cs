using System.Linq;

namespace Query
{
    public static class Downloads
    {
        public enum QueueStatus
        {
            queued = 0,
            pulled = 1, //when pulled from the queue to download
            downloaded = 2
        }

        public static void UpdateQueueItem(int qId, QueueStatus status = QueueStatus.downloaded)
        {
            Sql.ExecuteNonQuery("Download_Update", new { qId , status = (int)status });
        }

        public static int AddQueueItems(string[] urls, string domain, int feedId = 0)
        {
            //clean urls
            for(var x = 0; x < urls.Length; x++)
            {
                if(urls[x].Substring(urls[x].Length - 1, 1) == "/")
                {
                    urls[x] = urls[x].Substring(0, urls[x].Length - 1);
                }
            }
            return Sql.ExecuteScalar<int>("DownloadQueue_Add", new { urls = string.Join(",", urls), domain, feedId });
        }

        public static Models.DownloadQueue CheckQueue(int feedId = 0, int domaindelay = 60)
        {
            using(var conn = new Connection("DownloadQueue_Check", new { domaindelay, feedId }))
            {
                var results = conn.PopulateMultiple();
                var queue = results.Read<Models.DownloadQueue>().FirstOrDefault();
                if(queue != null)
                {
                    queue.downloadRules = results.Read<Models.DownloadRule>().ToList();
                }
                return queue;
            }
        }

        public static int Count()
        {
            return Sql.ExecuteScalar<int>("Downloads_GetCount");
        }
    }
}
