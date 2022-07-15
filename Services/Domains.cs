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
            if (info.paywall)
            {
                view.Show("subscription-required");
            }
            if (info.free)
            {
                view.Show("has-free-content");
            }
            var title = info.title != "" ? info.title : info.domain.GetDomainName();
            return JsonResponse(new { title, domainId = info.domainId, popup = view.Render() });
        }

        #region "Info"
        public string RequireSubscription(int domainId, bool required)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Query.Domains.RequireSubscription(domainId, required);
            return Success();
        }

        public string HasFreeContent(int domainId, bool free)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Query.Domains.HasFreeContent(domainId, free);
            return Success();
        }
        #endregion

        #region "Analyzer Rules"
        public string RenderAnalyzerRulesList(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/AnalyzerRules/rules.html");
            var item = new View("/Vendors/Collector/Views/AnalyzerRules/list-item.html");
            var rules = Query.Domains.AnalyzerRules.GetList(domainId);
            var html = new StringBuilder();
            foreach(var rule in rules)
            {
                item.Clear();
                item["ruleid"] = rule.ruleId.ToString();
                item["info"] = rule.selector;
                item["rule"] = rule.rule == true ? "protected" : "excluded";
                html.Append(item.Render());
            }
            view["rules"] = html.ToString();
            return view.Render();
        }

        public string AddAnalyzerRule(int domainId, string selector, bool protect)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if(selector == "")
            {
                return Error("You must provide a CSS selector");
            }
            Query.Domains.AnalyzerRules.Add(domainId, selector, protect);
            return RenderAnalyzerRulesList(domainId);
        }

        public string RemoveAnalyzerRule(int ruleId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Query.Domains.AnalyzerRules.Remove(ruleId);
            return Success();
        }
        #endregion

        #region "Download Rules"
        public string RenderDownloadRulesList(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/DownloadRules/rules.html");
            var item = new View("/Vendors/Collector/Views/DownloadRules/list-item.html");
            var rules = Query.Domains.DownloadRules.GetList(domainId);
            var html = new StringBuilder();
            foreach (var rule in rules)
            {
                item.Clear();
                item["ruleid"] = rule.ruleId.ToString();
                var info = new StringBuilder();
                if(rule.url != "") 
                {
                    info.Append("URL: " + rule.url);
                }
                if (rule.title != "")
                {
                    info.Append("Title: " + rule.title);
                }
                if (rule.summary != "")
                {
                    info.Append("Summary: " + rule.summary);
                }
                item["info"] = string.Join(", ", info);
                item["rule"] = rule.rule == true ? "Get links only" : "Do not download";
                html.Append(item.Render());
            }
            view["rules"] = html.ToString();
            return view.Render();
        }

        public string AddDownloadRule(int domainId, bool rule, string url, string title, string summary)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (url == "" && title == "" && summary == "")
            {
                return Error("You must provide at least one regular expression (URL, title, or summary)");
            }
            Query.Domains.DownloadRules.Add(domainId, rule, url, title, summary);
            return RenderDownloadRulesList(domainId);
        }

        public string RemoveDownloadRule(int ruleId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Query.Domains.DownloadRules.Remove(ruleId);
            return Success();
        }

        public string RenderCleanupDownloads(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/DownloadRules/cleanup.html");
            var viewArticle = new View("/Vendors/Collector/Views/DownloadRules/cleanup-article.html");
            var info = Query.Domains.GetById(domainId);
            var clean = Query.Domains.GetDownloadsToClean(domainId, true);
            view["domain"] = info.domain;
            view["total-articles"] = clean.totalArticles.ToString();
            view["total-downloads"] = clean.totalDownloads.ToString();
            if(clean.totalArticles > 0)
            {
                var html = new StringBuilder();
                foreach (var article in clean.articles)
                {
                    viewArticle.Clear();
                    viewArticle["title"] = article.title;
                    html.Append(viewArticle.Render());
                }
                view["articles"] = html.ToString();
                view.Show("has-articles");
            }
            return view.Render();
        }

        public string CleanupDownloads(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Domains.CleanupDownloads(domainId);
            return Success();
        }
        #endregion

        #region "Advanced"

        public string RenderAdvanced(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Domain/advanced.html");
            var domain = Query.Domains.GetById(domainId);
            if(domain.whitelisted == true)
            {
                view.Show("remove-whitelist");
            }
            else
            {
                view.Show("whitelist");
            }
            if (domain.blacklisted == true)
            {
                view.Show("remove-blacklist");
            }
            else
            {
                view.Show("blacklist");
            }
            return view.Render();
        }

        public string DeleteAllArticles(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Domains.DeleteAllArticles(domainId);
            return Success();
        }

        public string GetDomainTitle(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Query.Domains.FindDomainTitle(domainId);
        }

        public string Whitelist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == "") { return Error("No domain specified"); }
            Query.Whitelists.Domains.Add(domain);
            return Success();
        }

        public string RemoveWhitelist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == "") { return Error("No domain specified"); }
            Query.Whitelists.Domains.Remove(domain);
            return Success();
        }

        public string Blacklist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if(domain == "") { return Error("No domain specified"); }
            var info = Query.Domains.GetInfo(domain);
            Domains.DeleteAllArticles(info.domainId);
            Query.Blacklists.Domains.Add(domain);
            return Success();
        }

        public string RemoveBlacklist(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == "") { return Error("No domain specified"); }
            Query.Blacklists.Domains.Remove(domain);
            return Success();
        }
        #endregion
    }
}
