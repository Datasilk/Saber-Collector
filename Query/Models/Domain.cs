using System;

namespace Query.Models
{
    public class Domain
    {
        public int domainId { get; set; }
        public bool paywall { get; set; }
        public bool free { get; set; }
        public string domain { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime lastchecked { get; set; }
        public int articles { get; set; }
        public bool whitelisted { get; set; }
        public bool blacklisted { get; set; }
    }
}
