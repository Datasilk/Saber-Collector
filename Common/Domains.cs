using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
            newest = 3,
            paywall = 4,
            free = 5
        };
        public enum Sort
        {
            Ascending = 0,
            Descending = 1,
            TotalArticles = 2,
            Newest = 3,
            Oldest = 4
        };

        public static string RenderList(int subjectId = 0, Type type = 0, Sort sort = Sort.TotalArticles, int start = 1, int length = 50, string search = "")
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
                    item["domainid"] = domain.domainId.ToString();
                    if(domain.articles > 0)
                    {
                        item["total-articles"] = domain.articles.ToString();
                        item.Show("articles");
                    }
                    if(domain.whitelisted == true)
                    {
                        item.Show("whitelisted");
                    }
                    if (domain.paywall == true)
                    {
                        item.Show("paywall");
                    }
                    if (domain.free == true)
                    {
                        item.Show("free");
                    }
                    html.Append(item.Render());
                }
            }
            return html.ToString();
        }

        public static bool CheckDownloadRule(string urlMatch, string titleMatch, string summaryMatch, string url, string title, string summary)
        {
            if (urlMatch != "" && url != "")
            {
                urlMatch = urlMatch.ToLower();
                url = url.ToLower();
            }
            if(titleMatch != "" && title != "")
            {
                titleMatch = titleMatch.ToLower();
                title = title.ToLower();
            }
            if(summaryMatch != "" && summary != "")
            {
                summaryMatch = summaryMatch.ToLower();
                summary = summary.ToLower();
            }
            if ((urlMatch != "" && url != "" && url.IndexOf(urlMatch) > -1) || 
                (titleMatch != "" && title != "" && title.IndexOf(titleMatch) > -1) || 
                (summaryMatch != "" && summary != "" && summary.IndexOf(summaryMatch) > -1))
            {
                return true;
            }
            return false;
        }

        public static void CleanupDownloads(int domainId)
        {
            var clean = Query.Domains.GetDownloadsToClean(domainId);
            foreach(var article in clean.articles)
            {
                var domain = article.url.GetDomainName();
                var filename = "\\Content\\Collector\\articles\\" + domain.Substring(0, 2) + "\\" + domain + "\\" + article.articleId + ".html";
                if (File.Exists(App.MapPath(filename)))
                {
                    try
                    {
                        File.Delete(App.MapPath(filename));
                    }
                    catch (Exception) { }
                }
            }
            Query.Domains.CleanDownloads(domainId);
        }

        public static void DeleteAllArticles(int domainId)
        {
            var domain = Query.Domains.GetById(domainId);
            var folder = "\\Content\\Collector\\articles\\" + domain.domain.Substring(0, 2) + "\\" + domain + "\\";
            try
            {

                if (Directory.Exists(App.MapPath(folder)))
                {
                    try
                    {
                        Directory.Delete(App.MapPath(folder), true);
                    }
                    catch (Exception) { }
                }
            }
            catch(Exception ex)
            {
                Query.Logs.LogError(0, "", "DeleteAllArticles", ex.Message, ex.StackTrace);
            }

            folder = "\\wwwroot\\content\\collector\\articles\\" + domain.domain.Substring(0, 2) + "\\" + domain + "\\";
            try
            {
                if (Directory.Exists(App.MapPath(folder)))
                {
                    try
                    {
                        Directory.Delete(App.MapPath(folder), true);
                    }
                    catch (Exception) { }
                }
            }
            catch (Exception ex)
            {
                Query.Logs.LogError(0, "", "DeleteAllArticles", ex.Message, ex.StackTrace);
            }
            Query.Domains.DeleteAllArticles(domainId);
        }
    }
}
