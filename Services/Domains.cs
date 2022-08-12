using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Vendor;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorDomains : Service, IVendorService
    {
        #region "Add"
        public string RenderAdd()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Domains/modal-add.html");
            return view.Render();
        }

        public string Add(string domain, string title = "", int type = 0)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            if (domain == null || domain == "") { return Error("Please specify the domain you wish to add"); }
            try
            {
                domain = domain.GetDomainName();
                var domainId = Query.Domains.Add(domain, title, type);
                return GetDomainListItem(domainId);
            }
            catch(Exception ex)
            {
                return Error(ex.Message);
            }

        }
        #endregion

        #region "Search"
        public string Search(int subjectId, Query.Models.DomainType type, Query.Models.DomainSort sort, string search, int start, int length)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Domains.RenderComponent(subjectId, type, sort, start, length, search.Replace("*", "%"));
        }

        public string GetDomainListItem(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Domains.RenderListItem(domainId);
        }
        #endregion

        public string Details(string domain)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Domain/modal.html");
            var info = Query.Domains.GetInfo(domain);
            view["description"] = info.description != "" ? info.description : "No description was found for this domain yet.";
            view["domain-url"] = "https://" + info.domain;
            view["domain-type"] = ((int)info.type).ToString();
            view["domainid"] = info.domainId.ToString();
            view["articles"] = info.articles.ToString();
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

        public string UpdateDomainType(int domainId, int type)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Query.Domains.UpdateDomainType(domainId, (Query.Models.DomainType)type);
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

        public string GetDomainTitle(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Query.Domains.FindDomainTitle(domainId);
        }

        public string GetDescription(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Query.Domains.FindDescription(domainId);
        }

        public string GetDomainSubjects(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }

            return "";
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

        public string DeleteAllArticles(int domainId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            Domains.DeleteAllArticles(domainId);
            return Success();
        }
        #endregion

        #region "Collections"
        public string RenderCollections()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var view = new View("/Vendors/Collector/Views/Domains/collections.html");
            var item = new View("/Vendors/Collector/Views/Domains/collection-item.html");
            var groupView = new View("/Vendors/Collector/Views/Domains/collection-group.html");
            var html = new StringBuilder();
            var itemhtml = new StringBuilder();
            var collections = Query.Domains.Collections.GetList();
            var currentGroupId = 0;
            foreach(var collection in collections.Collections)
            {
                if(currentGroupId > 0 && currentGroupId != collection.colgroupId)
                {
                    //new group
                    var group = collections.Groups.Where(a => a.colgroupId == currentGroupId).FirstOrDefault();
                    if(group != null)
                    {
                        groupView.Clear();
                        groupView["name"] = group.name;
                        groupView["list"] = itemhtml.ToString();
                        html.Append(groupView.Render());
                        itemhtml.Clear();
                    }
                    else
                    {
                        //collection has no group
                        groupView.Clear();
                        groupView["name"] = "";
                        groupView["list"] = itemhtml.ToString();
                        html.Append(groupView.Render());
                        itemhtml.Clear();
                    }
                }
                item.Clear();
                item["search"] = collection.search;
                item["subjectid"] = collection.subjectId.ToString();
                item["type"] = ((int)collection.type).ToString();
                item["sort"] = ((int)collection.sort).ToString();
                item["title"] = collection.name;
                currentGroupId = collection.colgroupId;
                itemhtml.Append(item.Render());
            }
            //last group
            var group2 = collections.Groups.Where(a => a.colgroupId == currentGroupId).FirstOrDefault();
            if (group2 != null)
            {
                groupView.Clear();
                groupView["name"] = group2.name;
                groupView["list"] = itemhtml.ToString();
                html.Append(groupView.Render());
            }
            else if(itemhtml.Length > 0)
            {
                //collection has no group
                groupView.Clear();
                groupView["name"] = "";
                groupView["list"] = itemhtml.ToString();
                html.Append(groupView.Render());
            }
            view["list"] = html.ToString();
            return view.Render();
        }

        public string AddCollection(int colgroupId, string name, string search, int subjectId, Query.Models.DomainType type, Query.Models.DomainSort sort)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                if(name == "") { return Error("Collection Name is required"); }
                Query.Domains.Collections.Add(colgroupId, name, search, subjectId, type, sort);
                return Success();
            }
            catch (Exception) { return Error("Could not create new collection"); }
        }
        #endregion

        #region "Collection Groups"
        public string RenderGroups()
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var groups = Query.Domains.CollectionGroups.GetList();
            var html = new StringBuilder("<option value=\"0\">[No Category]</option>");
            foreach(var group in groups)
            {
                html.Append("<option value=\"" + group.colgroupId + "\">" + group.name + "</option>");
            }
            return html.ToString();
        }

        public string AddGroup(string name)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            try
            {
                if(name == "") { return Error("Name cannot be empty"); }
                Query.Domains.CollectionGroups.Add(name);
                return Success();
            }
            catch (Exception) { return Error("Could not create new domain collection category"); }
        }
        #endregion
    }
}
