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

        public static int AddQueueItems(string[] urls, string domain, int parentId = 0, int feedId = 0)
        {
            var count = Sql.ExecuteScalar<int>("DownloadQueue_BulkAdd", new { urls = string.Join(",", urls), domain, parentId, feedId });
            return count;
        }

        public static Int64 AddQueueItem(string url, string domain, int parentId = 0, int feedId = 0)
        {
            return Sql.ExecuteScalar<Int64>("DownloadQueue_Add", new { url, domain, parentId, feedId });
        }

        public enum QueueSort
        {
            Newest = 0,
            Oldest = 1,
            HomePages = 2,
            Random = 3
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

        public static void Archive(long qid)
        {
            Sql.ExecuteNonQuery("DownloadQueue_Archive", new { qid });
        }

        public static void MoveArchived()
        {
            Sql.ExecuteNonQuery("DownloadQueue_MoveArchived");
        }
    }
}
