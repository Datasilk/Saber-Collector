using System;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorDownloads : Service, IVendorService
    {

        #region "Whitelist"
        public string RenderWhitelist()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Downloads/whitelist.html");
            var item = new View("/Vendors/Collector/Views/Downloads/whitelist-item.html");
            var whitelists = Query.Whitelists.Domains.GetList();
            var html = new StringBuilder();
            foreach(var whitelist in whitelists)
            {
                item.Clear();
                item["domain"] = whitelist;
                item["url"] = "https://" + whitelist;
                html.Append(item.Render());
            }
            view["list"] = html.ToString();
            return view.Render();
        }

        public string DeleteWhitelist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == null || domain == "") { return Error("Missing domain"); }
            Query.Whitelists.Domains.Remove(domain);
            return Success();
        }
        #endregion

        #region "Blacklist"
        public string RenderBlacklist()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Downloads/blacklist.html");
            var item = new View("/Vendors/Collector/Views/Downloads/blacklist-item.html");
            var blacklists = Query.Blacklists.Domains.GetList();
            var html = new StringBuilder();
            foreach (var blacklist in blacklists)
            {
                item.Clear();
                item["domain"] = blacklist;
                item["url"] = "https://" + blacklist;
                html.Append(item.Render());
            }
            view["list"] = html.ToString();
            return view.Render();
        }

        public string DeleteBlacklist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == null || domain == "") { return Error("Missing domain"); }
            Query.Blacklists.Domains.Remove(domain);
            return Success();
        }
        #endregion

        #region "Acceptable Languages"
        public string RenderAcceptableLanguages()
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            return Error();
        }
        #endregion
    }
}
