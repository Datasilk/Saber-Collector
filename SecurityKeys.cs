using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.MarketManager
{
    public class SecurityKeys : IVendorKeys
    {
        public string Vendor { get; set; } = "Collector";
        public SecurityKey[] Keys { get; set; } = new SecurityKey[]
        {
            new SecurityKey(){Value = "run-queue", Label = "Run Download Queue", Description = "Allow users to run the download queue for RSS feeds & web scraping"},
        };
    }
}
