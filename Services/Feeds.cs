using System;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorFeeds : Service, IVendorService
    {
        public string Add(int categoryId, string title, string url, int intervals = 720, string filter = "")
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                Query.Feeds.Add(categoryId, title, url, filter, intervals);
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
