using System;

namespace Query.Models
{
    public class Domain
    {
        public int domainId { get; set; }
        public bool paywall { get; set; }
        public bool free { get; set; }
        public bool https { get; set; }
        public bool www { get; set; }
        public bool empty { get; set; }
        public bool deleted { get; set; }
        public DomainType type { get; set; }
        public string domain { get; set; }
        public string lang { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public DateTime lastchecked { get; set; }
        public int articles { get; set; }
        public int inqueue { get; set; }
        public bool whitelisted { get; set; }
        public bool blacklisted { get; set; }
    }

    public enum DomainFilterType
    {
        All = 0,
        Whitelisted = 1,
        Blacklisted = 2,
        Unlisted = 3,
        Paywall = 4,
        Free = 5,
        Unprocessed = 6,
        Empty = 7,
        Blacklist_Wildcard = 8,
        NotEmpty = 9
    }

    public enum DomainType
    {
        all = -1,
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
        crypto = 16,
        law = 17,
        medical = 18,
        political = 19,
        software_development = 20,
        photo_gallery = 21,
        videos = 22,
        music = 23,
        maps = 24,
        mobile_apps = 25,
        video_games = 26,
        erotic = 27,
        conspiracy = 28,
        religion = 29,
        weather = 30,
        comics = 31,
        gore = 32,
        real_estate = 33,
        _3d_animation = 34,
        live_streaming = 35,
        history = 36,
        guns_weapons = 37,
        magazine = 38,
        space = 39,
        directory = 40,
        propaganda = 41
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
