using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Saber.Vendors.Collector
{
    public static class Blacklist
    {
        public static List<Regex> Wildcards { get; set; } = new List<Regex>();
    }

}
