using System;
using System.Collections.Generic;
using System.Linq;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorAnalyzer : Service, IVendorService
    {

        public string AddCommonWords(List<string> words)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                words = words.Select(a => a.ToLower().Trim()).Where(a => a != "" && a.Length > 1).ToList();
                Query.CommonWords.Add(words.ToArray());

                var list = Models.Article.Rules.commonWords.ToList();
                list.AddRange(words);
                Models.Article.Rules.commonWords = list.ToArray();
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }
            return Success();
        }
    }
}
