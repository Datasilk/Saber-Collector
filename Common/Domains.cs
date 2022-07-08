using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector
{
    public static class Domains
    {
        public enum Type
        {
            all = 0,
            whitelisted = 1,
            blacklisted = 2,
            newest = 3
        };
        public enum Sort
        {
            Ascending = 0,
            Descending = 1,
            TotalArticles = 2,
            Newest = 3,
            Oldest = 4
        };

        public static string RenderList(int subjectId = 0, Type type = 0, Sort sort = Sort.Ascending, int start = 1, int length = 50, string search = "")
        {
            List<Query.Models.Domain> domains;
            var subjectIds = new List<int>();
            if(subjectId > 0)
            {
                subjectIds.Add(subjectId);
            }
            domains = Query.Domains.GetList(subjectIds.ToArray(), (int)type, (int)sort, search, start, length);

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
                    item["domain"] = domain.domain;
                    if(domain.articles > 0)
                    {
                        item["total-articles"] = domain.articles.ToString();
                        item.Show("articles");
                    }
                    if(domain.whitelisted == true)
                    {
                        item.Show("whitelisted");
                    }
                    html.Append(item.Render());
                }
            }
            return html.ToString();
        }

        public static bool CheckDownloadRule(string urlMatch, string titleMatch, string summaryMatch, string url, string title, string summary)
        {
            urlMatch = urlMatch.ToLower();
            titleMatch = titleMatch.ToLower();
            summaryMatch = summaryMatch.ToLower();
            url = url.ToLower();
            title = title.ToLower();
            summary = summary.ToLower();
            if (url.IndexOf(urlMatch) > -1 || title.IndexOf(titleMatch) > -1 || summary.IndexOf(summaryMatch) > -1)
            {
                return true;
            }
            return false;
        }
    }
}
