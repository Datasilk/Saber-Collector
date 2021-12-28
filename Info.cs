using Saber.Vendor;

namespace Saber.Vendors.Collector
{
    public class Info : IVendorInfo
    {
        public string Key { get; set; } = "Collector";
        public string Name { get; set; } = "Collector";
        public string Description { get; set; } = "Scrape & archive content from the web & RSS feeds";
        public string Icon { get; set; }
        public Version Version { get; set; } = "0.1.0.0";
    }
}
