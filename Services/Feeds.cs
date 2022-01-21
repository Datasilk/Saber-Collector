using System;
using Saber.Vendor;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorFeeds : Service, IVendorService
    {
        public string Add(int doctype, int categoryId, string title, string url, int intervals = 720, string filter = "")
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                var uri = Web.CleanUrl(url);
                var domain = uri.GetDomainName();
                Query.Feeds.Add((Query.Feeds.DocTypes)doctype, categoryId, title, url, domain, filter, intervals);
                return Success();
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }

        public string AddCategory(string title)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Query.Feeds.AddCategory(title);
                var categories = Query.Feeds.GetCategories();
                return Feeds.RenderOptions(categories);
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
    }
}
