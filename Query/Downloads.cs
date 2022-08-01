using System;
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

        public static void UpdateQueueItem(long qId, QueueStatus status = QueueStatus.downloaded)
        {
            Sql.ExecuteNonQuery("Download_Update", new { qId , status = (int)status });
        }

        public static void UpdateUrl(long qId, string url, string domain)
        {
            Sql.ExecuteNonQuery("Download_UpdateUrl", new { qId, url, domain });
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
            return Sql.ExecuteScalar<int>("DownloadQueue_BulkAdd", new { urls = string.Join(",", urls), domain, feedId });
        }

        public static Int64 AddQueueItem(string url, string domain, int feedId = 0)
        {
            return Sql.ExecuteScalar<Int64>("DownloadQueue_Add", new { url, domain, feedId });
        }

        public enum QueueSort
        {
            Newest = 0,
            Oldest = 1
        }

        public static Models.DownloadQueue CheckQueue(int feedId = 0, string domain = "", int domaindelay = 60, QueueSort sort = QueueSort.Newest)
        {
            using(var conn = new Connection("DownloadQueue_Check", new { domaindelay, domain, feedId, sort = (int)sort }))
            {
                try
                {
                    var results = conn.PopulateMultiple();
                    var queue = results.Read<Models.DownloadQueue>().FirstOrDefault();
                    if (queue != null)
                    {
                        queue.downloadRules = results.Read<Models.DownloadRule>().ToList();
                    }
                    return queue;
                }
                catch (Exception ex) 
                { 
                }
                return null;
            }
        }

        public static int Count()
        {
            return Sql.ExecuteScalar<int>("Downloads_GetCount");
        }

        public static void Delete(long qid)
        {
            Sql.ExecuteNonQuery("Download_Delete", new { qid });
        }

        public static void Move(long qid)
        {
            Sql.ExecuteNonQuery("DownloadQueue_Move", new { qid });
        }
    }
}
