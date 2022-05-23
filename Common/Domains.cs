using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector
{
    public static class Domains
    {
        public static string RenderList(int subjectId = 0, int start = 1, int length = 50, string search = "")
        {
            List<Query.Models.Domain> domains;
            var subjectIds = new List<int>();
            if(subjectId > 0)
            {
                subjectIds.Add(subjectId);
            }
            domains = Query.Domains.GetList(subjectIds.ToArray(), search);

            var item = new View("/Vendors/Collector/HtmlComponents/Domains/list-item.html");
            var html = new StringBuilder();
            if (domains != null)
            {
                foreach (var domain in domains)
                {
                    //populate view with domain info
                    item.Clear();
                    item["title"] = domain.title != "" ? domain.title : domain.domain.GetDomainName();
                    item["url"] = "https://" + domain.domain;
                    html.Append(item.Render());
                }
            }
            return html.ToString();
        }
    }
}
