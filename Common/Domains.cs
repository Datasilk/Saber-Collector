using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector
{
    public static class Domains
    {

        #region "Domains Component"
        public static string RenderComponent(int subjectId = 0, Query.Models.DomainFilterType type = 0, Query.Models.DomainType domainType = Query.Models.DomainType.all,  Query.Models.DomainSort sort = 0, int start = 1, int length = 200, string search = "")
        {
            var viewComponent = new View("/Vendors/Collector/HtmlComponents/Domains/htmlcomponent.html");
            var subjectIds = new List<int>();
            if (subjectId > 0)
            {
                subjectIds.Add(subjectId);
            }
            var total = Query.Domains.GetCount(subjectIds.ToArray(), type, domainType, sort, search);
            viewComponent["total-domains"] =  total.ToString("N0");
            viewComponent["pos-start"] = start.ToString("N0");
            viewComponent["pos-end"] = (start + length - 1).ToString("N0");
            viewComponent["prev-start"] = (start - length).ToString("");
            viewComponent["next-start"] = (start + length).ToString("");
            if (start == 1) 
            { 
                viewComponent.Show("no-prev"); 
            } 
            else 
            { 
                viewComponent.Show("has-paging");
                viewComponent.Show("show-first");
            }
            viewComponent["content"] = Components.Accordion.Render("Domains", "domains", RenderList(out var totalResults, subjectId, type, domainType, sort, start, length, search));
            if(totalResults < length)
            {
                viewComponent["pos-end"] = (start + totalResults - 1).ToString("N0");
                viewComponent.Show("no-next");
            }
            else
            {
                viewComponent.Show("has-paging");
            }
            return viewComponent.Render();
        }

        public static string RenderList(out int total, int subjectId = 0, Query.Models.DomainFilterType type = 0, Query.Models.DomainType domainType = 0, Query.Models.DomainSort sort = 0, int start = 1, int length = 200, string search = "")
        {
            var subjectIds = new List<int>();
            if(subjectId > 0)
            {
                subjectIds.Add(subjectId);
            }
            var domains = Query.Domains.GetList(subjectIds.ToArray(), type, domainType, sort, search, start, length);
            total = domains.Count;
            var item = new View("/Vendors/Collector/HtmlComponents/Domains/list-item.html");
            var html = new StringBuilder();
            if (domains != null)
            {
                foreach (var domain in domains)
                {
                    //populate view with domain info
                    item.Clear();
                    html.Append(RenderListItem(domain, item, type));
                }
            }
            return html.ToString();
        }

        public static string RenderListItem(int domainId, Query.Models.DomainFilterType type = 0)
        {
            var domain = Query.Domains.GetById(domainId);
            return RenderListItem(domain, null, type);
        }

        public static string RenderListItem(Query.Models.Domain domain, View item = null, Query.Models.DomainFilterType type = 0)
        {
            if (item == null) { item = new View("/Vendors/Collector/HtmlComponents/Domains/list-item.html"); }
            //populate view with domain info
            item["title"] = domain.title != null && domain.title != "" ? domain.title : domain.domain;
            //item["summary"] = domain.description.Length > 100 ? domain.description.Substring(0, 98) + "..." : domain.description;
            if (type != Query.Models.DomainFilterType.Blacklist_Wildcard)
            {
                item.Show("has-url");
            }
            item["url"] = "http" + (domain.https ? "s" : "") + "://" + (domain.www ? "www." : "") + domain.domain;
            item["domain"] = domain.domain;
            item["domainid"] = type == Query.Models.DomainFilterType.Blacklisted ? "-1" : domain.domainId.ToString();
            item["filter-type"] = type.ToString().ToLower();

            if ((int)domain.type > -1)
            {
                string domaintype;
                switch (domain.type)
                {
                    case Query.Models.DomainType.ecommerce:
                        domaintype = "e-commerce";
                        break;
                    case Query.Models.DomainType.science_journal:
                        domaintype = "science journal";
                        break;
                    case Query.Models.DomainType.search_engine:
                        domaintype = "search engine";
                        break;
                    case Query.Models.DomainType.ebooks:
                        domaintype = "e-books";
                        break;
                    case Query.Models.DomainType.software_development:
                        domaintype = "software dev";
                        break;
                    case Query.Models.DomainType.photo_gallery:
                        domaintype = "photo gallery";
                        break;
                    case Query.Models.DomainType.mobile_apps:
                        domaintype = "mobile apps";
                        break;
                    case Query.Models.DomainType.video_games:
                        domaintype = "video games";
                        break;
                    case Query.Models.DomainType._3d_animation:
                        domaintype = "3D animation";
                        break;
                    case Query.Models.DomainType.live_streaming:
                        domaintype = "live streams";
                        break;
                    case Query.Models.DomainType.guns_weapons:
                        domaintype = "guns & weapons";
                        break;
                    default:
                        domaintype = domain.type.ToString();
                        break;
                }
                item.Show("has-domain-type");
                item["domain-type"] = domaintype;
                item["domain-type-id"] = domain.type.ToString();
            }else if(type == Query.Models.DomainFilterType.Blacklisted)
            {
                item["domain-type"] = "blacklisted";
                item["domain-type-id"] = "blacklisted";
            }
            if (domain.articles > 0)
            {
                item["total-articles"] = domain.articles.ToString();
                item.Show("articles");
            }
            if (domain.whitelisted == true)
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
            return item.Render();
        }
        #endregion

        #region "Download Rules"
        public static int DownloadMinimumIntervals { get; set; } = 60; //in seconds

        public class DownloadRuleMatch
        {
            public string Url { get; set; } = "";
            public string Title { get; set; } = "";
            public string Summary { get; set; } = "";
        }

        //set of default download rules for downloads that will be kept and used only to collect URL links from
        public static DownloadRuleMatch[] DefaultDownloadLinksOnlyRules = new DownloadRuleMatch[]
        {
            new DownloadRuleMatch(){Url = "/page/"},
            new DownloadRuleMatch(){Url = "/category/"},
            new DownloadRuleMatch(){Url = "/subject/"},
            new DownloadRuleMatch(){Url = "/subjects/"},
            new DownloadRuleMatch(){Url = "/sitemap/"},
            new DownloadRuleMatch(){Url = "/tag/"},
            new DownloadRuleMatch(){Url = "/tags"},
            new DownloadRuleMatch(){Url = "/genre/"},
            new DownloadRuleMatch(){Url = "/genres/"}
        };

        //set of default download rules for URLs that will not be downloaded or used as articles
        public static DownloadRuleMatch[] DefaultDoNotDownloadRules = new DownloadRuleMatch[]
        {
            new DownloadRuleMatch(){Url = "/sponsor"},
            new DownloadRuleMatch(){Url = "/metrics"},
            new DownloadRuleMatch(){Url = ".jpg"},
            new DownloadRuleMatch(){Url = "/cart"},
            new DownloadRuleMatch(){Url = "/order/"},
            new DownloadRuleMatch(){Url = "/shop/"},
            new DownloadRuleMatch(){Title = "page not found"},
            new DownloadRuleMatch(){Title = "file not found"},
            new DownloadRuleMatch(){Title = "sign in"},
            new DownloadRuleMatch(){Title = "log in"},
            new DownloadRuleMatch(){Title = "access denied"},
            new DownloadRuleMatch(){Title = "404"},
        };

        public static bool CheckDefaultDownloadLinksOnlyRules(string url, string title, string summary)
        {
            foreach(var rule in DefaultDownloadLinksOnlyRules)
            {
                if(CheckDownloadRule(rule.Url, rule.Title, rule.Summary, url, title, summary) == true)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool CheckDefaultDoNotDownloadRules(string url, string title, string summary)
        {
            foreach (var rule in DefaultDoNotDownloadRules)
            {
                if (CheckDownloadRule(rule.Url, rule.Title, rule.Summary, url, title, summary) == true)
                {
                    return true;
                }
            }
            return false;
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

        #endregion

        #region "Validation"
        public static bool ValidateURL(string url)
        {
            bool isValid = url.IndexOf("http://") == 0 || url.IndexOf("https://") == 0;
            if (isValid)
            {
                url = url.Replace("http://", "").Replace("https://", "").Replace("www.", "");
                var parts = url.Split('/');
                if(parts.Length > 0)
                {
                    parts = parts[0].Split(".");
                    if(parts.Length >= 2 && parts[0].Length > 0 && parts[1].Length > 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public static bool ValidateDomain(string domain)
        {
            //check blacklist wildcards
            foreach (var wildcard in Blacklist.Wildcards)
            {
                if (wildcard.Match(domain).Success)
                {
                    return false;
                }
            }
            var parts = domain.Split(".");
            if(parts.Length < 2) { return false; }
            if (int.TryParse(parts[parts.Length - 1], out int num))
            {
                //found potential IP address
                return false;
            }
            return true;
        }
        #endregion

        #region "Delete"
        public static void DeleteAllArticles(int domainId)
        {
            var domain = Query.Domains.GetById(domainId);
            if(domain.domain != "")
            {
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
            }
            
            Query.Domains.DeleteAllArticles(domainId);
        }
        #endregion 
    }
}
