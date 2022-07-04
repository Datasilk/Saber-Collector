using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Saber.Core;
using Saber.Vendor;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorDomains : Service, IVendorService
    {

        public string Search(int subjectId, int type, int sort, string search, int start, int length)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Domains.RenderList(subjectId, (Domains.Type)type, (Domains.Sort)sort, start, length, search);
        }

        public string Details(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Domain/modal.html");
            var info = Query.Domains.GetInfo(domain);
            view["description"] = info.description != "" ? info.description : "No description was found for this domain yet.";
            view["domain-url"] = "https://" + info.domain;
            var title = info.title != "" ? info.title : info.domain.GetDomainName();
            return JsonResponse(new { title, domainId = info.domainId, popup = view.Render() });
        }

        public string RenderAnalyzerRulesList(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/AnalyzerRules/rules.html");
            var item = new View("/Vendors/Collector/Views/AnalyzerRules/list-item.html");

            return view.Render();
        }
    }
}
