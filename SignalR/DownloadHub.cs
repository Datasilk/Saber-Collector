using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Net;
using Microsoft.AspNetCore.SignalR;
using Saber.Vendors.Collector.Models.Article;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector.Hubs
{
    public class DownloadHub : Hub
    {
        private List<string> StopQueues = new List<string>();

        public async Task CheckQueue(string id)
        {
            var queue = Query.Downloads.CheckQueue(1); // 1 minute delay for each download on a single domain
            if(queue != null)
            {
                if (CheckToStopQueue(id, Clients.Caller)) { return; }
                AnalyzedArticle article = new AnalyzedArticle();
                await Clients.Caller.SendAsync("update", "Downloading <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>...");

                //download content //////////////////////////////////////////////////////
                var result = Article.Download(queue.url);
                if (CheckToStopQueue(id, Clients.Caller)) { return; }
                if (result == "")
                {
                    await Clients.Caller.SendAsync("update", "Download timed out for URL: <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>");
                    return;
                }
                try
                {
                    article = Html.DeserializeArticle(result);
                }
                catch (Exception)
                {
                    await Clients.Caller.SendAsync("update", "Error parsing DOM!");
                    await Clients.Caller.SendAsync("checked");
                    return;
                }
                if (CheckToStopQueue(id, Clients.Caller)) { return; }

                //save article
                Article.Add(queue.url);

                //get URLs from all anchor links on page //////////////////////////////////
                var urls = new Dictionary<string, List<string>>();
                var links = article.elements.Where(a => a.tagName == "a" && a.attribute.ContainsKey("href")).Select(a => a.attribute["href"]);
                var viableDownloads = 0;
                var addedLinks = 0;

                foreach (var url in links)
                {
                    if (CheckToStopQueue(id, Clients.Caller)) { return; }
                    if (string.IsNullOrEmpty(url)) { continue; }
                    var uri = Web.CleanUrl(url);
                    if (uri.StartsWith("mailto:")) { continue; }
                    if (uri.StartsWith("javascript:")) { continue; }
                    if(uri.Length > 255) { continue; }

                    var domain = uri.GetDomainName();
                    if (Models.Blacklist.Domains.Any(a => domain.IndexOf(a) == 0)) { continue; }
                    //if (!Models.Whitelist.Domains.Any(a => domain.IndexOf(a) == 0)) { continue; }
                    if (!urls.ContainsKey(domain))
                    {
                        urls.Add(domain, new List<string>());   
                    }
                    urls[domain].Add(uri);
                }
                foreach(var domain in urls.Keys)
                {
                    try
                    {
                        if (CheckToStopQueue(id, Clients.Caller)) { return; }
                        if (urls[domain] == null || urls[domain].Count == 0) { continue; }
                        var count = Query.Downloads.AddQueueItems(string.Join(",", urls[domain]), domain, queue.feedId);
                        if (count > 0)
                        {
                            addedLinks += count;
                            await Clients.Caller.SendAsync("update",
                                "<span>Found " + count + " new link(s) for <a href=\"https://" + domain + "\" target=\"_blank\">" + domain + "</a></span>" +
                                "<div class=\"col right\">" + 
                                (
                                    !Models.Whitelist.Domains.Any(a => domain.IndexOf(a) == 0) ? 
                                    "<a href=\"javascript:\" onclick=\"S.downloads.whitelist.add('" + domain + "')\"><small>whitelist</small></a> / " : ""
                                ) +
                                "<a href=\"javascript:\" onclick=\"S.downloads.blacklist.add('" + domain + "')\"><small>blacklist</small></a>" +
                                "</div>");
                        }
                    }
                    catch(Exception ex)
                    {
                        await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace + "<br/>" + 
                            domain + ", " + string.Join(",", urls[domain].Distinct().ToArray()));
                    }
                    
                }

                //process article /////////////////////////////////////////////////////
                if (CheckToStopQueue(id, Clients.Caller)) { return; }
                try
                {
                    var articleInfo = Article.AddFromAnalyzedArticle(queue.url, article);
                    if (articleInfo.wordcount > 50)
                    {
                        viableDownloads++;
                    }
                    await Clients.Caller.SendAsync("update", "<span>" +
                            "words: " + articleInfo.wordcount + ", sentences: " + articleInfo.sentencecount + ", important: " + articleInfo.importantcount + ", score: " + articleInfo.score +
                            "(" + (article.subjects.Count > 0 ? string.Join(", ", article.subjects.Select(a => a.title)) : "") + ") " +
                        "</span>");
                    await Clients.Caller.SendAsync("checked", 1, articleInfo.wordcount > 50 ? 1 : 0, addedLinks, articleInfo.wordcount ?? 0, articleInfo.importantcount ?? 0);
                    return;
                }
                catch(Exception ex)
                {
                    await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace);
                    await Clients.Caller.SendAsync("checked", 0, 0, 0, 0, 0);
                    return;
                }
            }
            else
            {
                await Clients.Caller.SendAsync("checked", 0, 0, 0, 0, 0);
            }
        }

        public async Task StopQueue(string id)
        {
            StopQueues.Add(id);
            await Clients.Caller.SendAsync("update", "Stopping download queue...");
        }

        private bool CheckToStopQueue(string id, IClientProxy Caller)
        {
            if (StopQueues.Contains(id))
            {
                StopQueues.Remove(id);
                Caller.SendAsync("update", "Stopped download queue");
                return true;
            }
            return false;
        }

        public async Task CheckFeeds()
        {
            var feeds = Query.Feeds.Check();
            foreach(var feed in feeds)
            {
                if(feed.doctype == Query.Feeds.DocTypes.RSS)
                {
                    //Read RSS feed ///////////////////////////////////////////////////////////////////////////////
                    using (var client = new WebClient())
                    {
                        var response = client.DownloadString(feed.url);
                        var content = Utility.Syndication.Read(response);
                        var links = content.items.Select(a => a.link);
                        var domains = new Dictionary<string, List<string>>();
                        //separate links by domain
                        foreach (var link in links)
                        {
                            var domain = link.GetDomainName();
                            if (!domains.ContainsKey(domain))
                            {
                                domains.Add(domain, new List<string>());
                            }
                            domains[domain].Add(link);
                        }
                        //add all links for all domains to download queue
                        var count = 0;
                        foreach (var domain in domains.Keys)
                        {
                            var dlinks = domains[domain];
                            if (dlinks.Count > 0)
                            {
                                count += Query.Downloads.AddQueueItems(string.Join(",", dlinks), domain);
                            }
                        }
                        if (count > 0)
                        {
                            await Clients.Caller.SendAsync("update", "Added " + count + " URLs to the download queue from feed " + feed.url);
                        }
                    }
                }
                else if(feed.doctype == Query.Feeds.DocTypes.HTML)
                {
                    //Read HTML feed //////////////////////////////////////////////////////////////////////////////
                    AnalyzedArticle article = new AnalyzedArticle();
                    var result = Article.Download(feed.url);
                    try
                    {
                        article = Html.DeserializeArticle(result);
                    }
                    catch (Exception)
                    {
                        await Clients.Caller.SendAsync("update", "Error parsing feed DOM for " + feed.url + "!");
                        continue;
                    }
                    var links = article.elements.Where(a => a.tagName == "a" && a.attribute.ContainsKey("href")).Select(a => a.attribute["href"]);
                    var urls = new Dictionary<string, List<string>>();

                    foreach (var url in links)
                    {
                        if (string.IsNullOrEmpty(url)) { continue; }
                        var uri = Web.CleanUrl(url);
                        if (uri.StartsWith("mailto:")) { continue; }
                        if (uri.StartsWith("javascript:")) { continue; }
                        if (uri.Length > 255) { continue; }

                        var domain = uri.GetDomainName();
                        if (Models.Blacklist.Domains.Any(a => domain.IndexOf(a) == 0)) { continue; }
                        //if (!Models.Whitelist.Domains.Any(a => domain.IndexOf(a) == 0)) { continue; }
                        if (!urls.ContainsKey(domain))
                        {
                            urls.Add(domain, new List<string>());
                        }
                        urls[domain].Add(uri);
                    }
                    foreach (var domain in urls.Keys)
                    {
                        try
                        {
                            if (urls[domain] == null || urls[domain].Count == 0) { continue; }
                            var count = Query.Downloads.AddQueueItems(string.Join(",", urls[domain]), domain, feed.feedId);
                            if (count > 0)
                            {
                                await Clients.Caller.SendAsync("update",
                                    "<span>Found " + count + " new link(s) for feed " + feed.title + ": <a href=\"https://" + domain + "\" target=\"_blank\">" + domain + "</a></span>" +
                                    "<div class=\"col right\">" +
                                    (
                                        !Models.Whitelist.Domains.Any(a => domain.IndexOf(a) == 0) ?
                                        "<a href=\"javascript:\" onclick=\"S.downloads.whitelist.add('" + domain + "')\"><small>whitelist</small></a> / " : ""
                                    ) +
                                    "<a href=\"javascript:\" onclick=\"S.downloads.blacklist.add('" + domain + "')\"><small>blacklist</small></a>" +
                                    "</div>");
                            }
                        }
                        catch (Exception ex)
                        {
                            await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace + "<br/>" +
                                domain + ", " + string.Join(",", urls[domain].Distinct().ToArray()));
                        }
                    }
                }
                Query.Feeds.UpdateLastChecked(feed.feedId);
            }
            await Clients.Caller.SendAsync("update", "Checked feeds.");
        }

        public async Task Whitelist(string domain)
        {
            Query.Whitelists.Domains.Add(domain);
            //add domain to white list object
            if (!Models.Whitelist.Domains.Contains(domain))
            {
                Models.Whitelist.Domains.Add(domain);
            }
            await Clients.Caller.SendAsync("update", "Whitelisted <a href=\"" + domain + "\" target=\"_blank\">" + domain + "</a> for the download queue");
        }

        public async Task Blacklist(string domain)
        {
            Query.Blacklists.Domains.Add(domain);
            try
            {
                //delete physical content for domain on disk
                Directory.Delete(Saber.App.MapPath("/Content/" + domain.Substring(0, 2) + "/" + domain), true);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("update", "Could not delete folder " + "/Content/" + domain.Substring(0, 2) + "/" + domain);
            }
            //add domain to black list object
            if (!Models.Blacklist.Domains.Contains(domain))
            {
                Models.Blacklist.Domains.Add(domain);
            }
            await Clients.Caller.SendAsync("update", "Blacklisted domain " + domain + " and removed all related articles");
        }
    }
}
