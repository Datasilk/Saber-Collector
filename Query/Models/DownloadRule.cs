using System;

namespace Query.Models
{
    public class DownloadRule
    {
        public int ruleId { get; set; }
        public int domainId { get; set; }
        public string url { get; set; }
        public string title { get; set; }
        public string summary { get; set; }
        public DateTime datecreated { get; set; }
    }
}
