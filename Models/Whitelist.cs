using System.Collections.Generic;

namespace Saber.Vendors.Collector.Models
{
    public static class Whitelist
    {
        public static List<string> Domains { get; set; } = new List<string>();

        public static void Update()
        {
            Domains = Query.Whitelists.Domains.GetList();
        }
    }
}
