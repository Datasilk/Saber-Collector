using System;

namespace Query.Models
{
    public class Domain
    {
        public int domainId { get; set; }
        public bool paywall { get; set; }
        public bool free { get; set; }
        public DomainType type { get; set; }
        public string domain { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime lastchecked { get; set; }
        public int articles { get; set; }
        public bool whitelisted { get; set; }
        public bool blacklisted { get; set; }
    }

    public enum DomainType
    {
        unused = 0,
        website = 1,
        ecommerce = 2,
        wiki = 3,
        blog = 4,
        science_journal= 5,
        SASS = 6,
        social_network = 7,
        advertiser = 8,
        search_engine = 9,
        portfolio = 10,
        news = 11,
        travel = 12,
        aggregator = 13,
        government = 14,
        ebooks = 15,
    }

    public enum DomainSort
    {
        Alphabetical = 0,
        AlphabeticalDescending = 1,
        MostArticles = 2,
        Newest = 3,
        Oldest = 4,
        LastUpdated = 5
    }
}
