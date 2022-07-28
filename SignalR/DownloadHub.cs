using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
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

        public async Task CheckQueue(string id, int feedId, string domainName, int sort)
        {
            try
            {

                var queue = Query.Downloads.CheckQueue(feedId, domainName, domainName != "" ? 5 : 10, (Query.Downloads.QueueSort)sort); // 10 second delay for each download on a single domain
                if (queue != null)
                {
                    if (CheckToStopQueue(id, Clients.Caller)) { return; }

                    if (!Domains.ValidateDomain(queue.domain))
                    {
                        //invalid domain, delete domain & all articles related to domain
                        //shouldn't happen but just in case
                        Domains.DeleteAllArticles(queue.domainId);
                        Query.Domains.Delete(queue.domainId);
                    }

                    if (!Domains.ValidateURL(queue.url))
                    {
                        //shouldn't happen but just in case
                        Query.Downloads.Delete(queue.qid);
                        await Clients.Caller.SendAsync("update", "Invalid URL");
                        await Clients.Caller.SendAsync("checked", 1);
                        return;
                    }

                    //first check download rules for queue
                    var downloadOnly = false;
                    foreach (var rule in queue.downloadRules)
                    {
                        if (rule.rule == false && rule.url != "" && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                        {
                            await Clients.Caller.SendAsync("update", "URL matches download rule \"" + rule.url + "\" and will be skipped (<a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>)");
                            await Clients.Caller.SendAsync("checked", 1);
                            return;
                        }
                        else if (rule.rule == true && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                        {
                            downloadOnly = true;
                        }
                    }

                    AnalyzedArticle article = new AnalyzedArticle();

                    await Clients.Caller.SendAsync("update", "Downloading <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>...");

                    //download content //////////////////////////////////////////////////////
                    var result = Article.Download(queue.url, out var newurl);
                    if (newurl != queue.url)
                    {
                        ////////////////////////////////////////////////////////////////////////////////
                        //updated URL, retire current download and create new download for the new URL
                        Query.Downloads.Move(queue.qid);
                        queue.qid = Query.Downloads.AddQueueItem(newurl, newurl.GetDomainName(), feedId);
                        queue.url = newurl;
                        var domain = Query.Domains.GetInfo(newurl.GetDomainName());
                        queue.domainId = domain.domainId;
                        queue.domain = domain.domain;
                        queue.downloadRules = Query.Domains.DownloadRules.GetList(domain.domainId);

                        //check download rules again (for new URL)
                        downloadOnly = false;
                        foreach (var rule in queue.downloadRules)
                        {
                            if (rule.rule == false && rule.url != "" && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                            {
                                await Clients.Caller.SendAsync("update", "URL matches download rule \"" + rule.url + "\" and will be skipped (<a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>)");
                                await Clients.Caller.SendAsync("checked", 1);
                                return;
                            }
                            else if (rule.rule == true && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                            {
                                downloadOnly = true;
                            }
                        }
                        ////////////////////////////////////////////////////////////////////////////////
                    }
                    if (downloadOnly == false)
                    {
                        downloadOnly = !Query.Whitelists.Domains.Check(queue.url.GetDomainName());
                    }

                    if (CheckToStopQueue(id, Clients.Caller)) { return; }
                    if (result == "")
                    {
                        await Clients.Caller.SendAsync("update", "Download timed out for URL: <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>");
                        await Clients.Caller.SendAsync("checked", 1);
                        return;
                    }
                    else if (result.Substring(0, 5) == "file:")
                    {
                        await Clients.Caller.SendAsync("update", "URL points to a file of type \"" + result.Substring(5) + "\"");
                        await Clients.Caller.SendAsync("checked", 1);
                        return;
                    }
                    try
                    {
                        article = Html.DeserializeArticle(result);
                        article.feedId = queue.feedId;
                    }
                    catch (Exception)
                    {
                        await Clients.Caller.SendAsync("update", "Error parsing DOM!");
                        await Clients.Caller.SendAsync("checked", 1);
                        return;
                    }

                    //process article /////////////////////////////////////////////////////
                    if (CheckToStopQueue(id, Clients.Caller)) { return; }
                    try
                    {
                        //merge analyzed article into Query Article
                        var existingArticle = Query.Articles.GetByUrl(queue.url) ?? Article.Create(queue.url);
                        var articleInfo = Article.Merge(existingArticle, article);

                        if(articleInfo.url.Length >= queue.domain.Length + 7 && 
                            articleInfo.url.Substring(articleInfo.url.IndexOf(queue.domain) + queue.domain.Length).Length <= 1)
                        {
                            //found home page
                            Query.Domains.UpdateDescription(queue.domainId, articleInfo.title, articleInfo.summary);
                        }

                        //check all download rules against article info
                        if(Domains.CheckDefaultDownloadLinksOnlyRules(queue.url, articleInfo.title, articleInfo.summary))
                        {
                            downloadOnly = true;
                        }
                        else
                        {
                            //check domain-specific download rules
                            foreach (var rule in queue.downloadRules)
                            {
                                if (rule.rule == true && Domains.CheckDownloadRule(rule.url, rule.title, rule.summary, queue.url, articleInfo.title, articleInfo.summary) == true)
                                {
                                    downloadOnly = true;
                                    break;
                                }
                            }
                        }


                        //get article score
                        Article.DetermineScore(article, articleInfo);

                        await Clients.Caller.SendAsync("update", "<span>" +
                                "words: " + articleInfo.wordcount + ", sentences: " + articleInfo.sentencecount + ", important: " + articleInfo.importantcount + ", score: " + articleInfo.score +
                                "(" + (article.subjects.Count > 0 ? string.Join(", ", article.subjects.Select(a => a.title)) : "") + ") " +
                            "</span>");

                        if (downloadOnly == false)
                        {
                            //save article to database
                            if(articleInfo.articleId <= 0)
                            {
                                //add article (which also archives download)
                                Article.Add(articleInfo);
                            }
                            else
                            {
                                //archive download
                                Query.Downloads.Move(queue.qid);
                            }

                            //save downloaded results to disk
                            var relpath = Article.ContentPath(queue.url);
                            var filepath = App.MapPath(relpath);
                            var filename = articleInfo.articleId + ".html";
                            if (!Directory.Exists(filepath))
                            {
                                //create folder for content
                                Directory.CreateDirectory(filepath);
                            }
                            File.WriteAllText(filepath + filename, result);
                        }
                        else
                        {
                            Query.Downloads.Move(queue.qid);
                        }

                        //display article information
                        await Clients.Caller.SendAsync("article", JsonSerializer.Serialize(articleInfo));

                        //get URLs from all anchor links on page //////////////////////////////////
                        var urls = new Dictionary<string, List<KeyValuePair<string, string>>>();
                        var links = Article.GetLinks(article);
                        var addedLinks = 0;

                        foreach (var link in links)
                        {
                            var url = link.attribute.ContainsKey("href") ? link.attribute["href"] : "";
                            if (CheckToStopQueue(id, Clients.Caller)) { return; }

                            //validate link url
                            if (string.IsNullOrEmpty(url)) { continue; }
                            var uri = Web.CleanUrl(url, false);
                            if (!ValidateURL(uri)) { continue; }
                            if (!Domains.ValidateURL(uri)) { continue; }
                            var domain = uri.GetDomainName();
                            //if (!Models.Whitelist.Domains.Any(a => domain.IndexOf(a) == 0)) { continue; }

                            if (!urls.ContainsKey(domain))
                            {
                                urls.Add(domain, new List<KeyValuePair<string, string>>());
                            }
                            var querystring = Web.CleanUrl(url, onlyKeepQueries: new string[] { "id=", "item" }).Replace(uri, "");
                            urls[domain].Add(new KeyValuePair<string, string>(uri, querystring));
                        }

                        //get all download rules for all domains found on the page
                        var downloadRules = new List<Query.Models.DownloadRule>();
                        if (urls.Keys.Count > 0)
                        {
                            downloadRules = Query.Domains.DownloadRules.GetForDomains(urls.Keys.ToArray());
                        }

                        //add all found links to the download queue
                        var keys = urls.Keys.ToArray();
                        for(var x = 0; x < keys.Length; x++)
                        {
                            var domain = keys[x];
                            try
                            {
                                if (CheckToStopQueue(id, Clients.Caller)) { return; }
                                if (Query.Blacklists.Domains.Check(domain)) { continue; }
                                var rules = downloadRules.Where(b => b.domain == domain);
                                if (urls[domain] == null || urls[domain].Count == 0) { continue; }

                                //filter URLs that pass the download rules
                                ValidateURLs(domain, downloadRules, urls, out var urlsChecked);

                                //add filtered URLs to download queue
                                var count = urlsChecked != null ? Query.Downloads.AddQueueItems(urlsChecked.ToArray(), domain, queue.feedId) : 0;
                                if (count > 0)
                                {
                                    addedLinks += count;
                                    await Clients.Caller.SendAsync("update",
                                        "<span>Found " + count + " new link(s) for <a href=\"https://" + domain + "\" target=\"_blank\">" + domain + "</a></span>");
                                }
                            }
                            catch (Exception ex)
                            {
                                await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace + "<br/>" +
                                    domain + ", " + string.Join(",", urls[domain].Distinct().ToArray()));
                            }
                        }

                        //finished processing download
                        await Clients.Caller.SendAsync("checked", 0, 1, downloadOnly == false && articleInfo.wordcount > 50 ? 1 : 0, addedLinks, articleInfo.wordcount ?? 0, articleInfo.importantcount ?? 0);
                        return;
                    }
                    catch (Exception ex)
                    {
                        await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace);
                        await Clients.Caller.SendAsync("checked", 1, 0, 0, 0, 0, 0);
                        return;
                    }
                }
                else
                {
                    await Clients.Caller.SendAsync("update", "No downloads queued at the moment...");
                    await Clients.Caller.SendAsync("checked", 0, 0, 0, 0, 0, 0);
                }
            }
            catch(Exception ex)
            {
                await Clients.Caller.SendAsync("update", "An error occurred while checking the download queue");
                await Clients.Caller.SendAsync("checked", 1, 0, 0, 0, 0, 0);
                Query.Logs.LogError(0, "", "DownloadHub", ex.Message, ex.StackTrace);
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

        public async Task CheckFeeds(int feedId)
        {
            var feeds = Query.Feeds.Check(feedId);
            await Clients.Caller.SendAsync("update", "Checking " + feeds.Count + " feed" + (feeds.Count != 1 ? "s" : "") + "...");
            var i = 0;
            var len = feeds.Count;
            foreach (var feed in feeds)
            {
                i++;
                if(feed.doctype == Query.Feeds.DocTypes.RSS)
                {
                    //Read RSS feed ///////////////////////////////////////////////////////////////////////////////
                    using (var client = new WebClient())
                    {
                        var response = client.DownloadString(feed.url);
                        var content = Utility.Syndication.Read(response);
                        var links = content.items.Select(a => a.link);
                        var urls = new Dictionary<string, List<KeyValuePair<string, string>>>();

                        //separate links by domain
                        foreach (var url in links)
                        {
                            //validate link url
                            if (string.IsNullOrEmpty(url)) { continue; }
                            var uri = Web.CleanUrl(url, false);
                            if (!ValidateURL(uri)) { continue; }
                            if (!Domains.ValidateURL(uri)) { continue; }
                            var domain = uri.GetDomainName();
                            if (!urls.ContainsKey(domain))
                            {
                                urls.Add(domain, new List<KeyValuePair<string, string>>());
                            }
                            var querystring = Web.CleanUrl(url, onlyKeepQueries: new string[] { "id=", "item" }).Replace(uri, "");
                            urls[domain].Add(new KeyValuePair<string, string>(uri, querystring));
                        }

                        //get all download rules for all domains found on the page
                        var downloadRules = new List<Query.Models.DownloadRule>();
                        if (urls.Keys.Count > 0)
                        {
                            downloadRules = Query.Domains.DownloadRules.GetForDomains(urls.Keys.ToArray());
                        }

                        //add all links for all domains to download queue
                        var count = 0;
                        foreach (var domain in urls.Keys)
                        {
                            if (Query.Blacklists.Domains.Check(domain)) { continue; }

                            //filter URLs that pass the download rules
                            ValidateURLs(domain, downloadRules, urls, out var urlsChecked);

                            var dlinks = urls[domain].Select(a => a.Key + a.Value);
                            if (dlinks.Count() > 0)
                            {
                                count += Query.Downloads.AddQueueItems(dlinks.ToArray(), domain, feed.feedId);
                            }
                        }
                        if (count > 0)
                        {
                            await Clients.Caller.SendAsync("feed", count, 
                                "<span>(" + i +" of " + len + ") Found " + count + "new link(s) from " + feed.title + ": <a href=\"" + feed.url + "\" target=\"_blank\">" + feed.url + "</a></span>");
                        }
                        else
                        {
                            await Clients.Caller.SendAsync("feed", count,
                                "<span>(" + i + " of " + len + " feeds) No new links from " + feed.title);
                        }
                    }
                }
                else if(feed.doctype == Query.Feeds.DocTypes.HTML)
                {
                    //Read HTML feed //////////////////////////////////////////////////////////////////////////////
                    AnalyzedArticle article = new AnalyzedArticle();
                    string result;
                    try
                    {
                        result = Article.Download(feed.url, out var newurl);
                        feed.url = newurl;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Query.Feeds.UpdateLastChecked(feed.feedId); //mark feed as checked (since it was attempted to be downloaded)
                        if (ex.Message.IndexOf("405") > 0)
                        {
                            await Clients.Caller.SendAsync("update", "405 Error downloading " + feed.url + "!");
                        }
                        else
                        {
                            await Clients.Caller.SendAsync("update", "Error downloading " + feed.url + "!");
                        }
                        continue;
                    }
                    try
                    {
                        article = Html.DeserializeArticle(result);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Console.WriteLine(ex.StackTrace);
                        Query.Feeds.UpdateLastChecked(feed.feedId); //mark feed as checked (since it was downloaded)
                        await Clients.Caller.SendAsync("update", "Error parsing feed DOM for " + feed.url + "!");
                        continue;
                    }
                    var links = article.elements.Where(a => a.tagName == "a" && a.attribute.ContainsKey("href"))
                        .Select(a => a.attribute["href"]);

                    var urls = new Dictionary<string, List<KeyValuePair<string, string>>>();

                    //separate links by domain
                    foreach (var url in links)
                    {
                        //validate link url
                        if (string.IsNullOrEmpty(url)) { continue; }
                        var uri = Web.CleanUrl(url, false);
                        if (!ValidateURL(uri)) { continue; }
                        if (!Domains.ValidateURL(uri)) { continue; }
                        var domain = uri.GetDomainName();

                        if (!urls.ContainsKey(domain))
                        {
                            urls.Add(domain, new List<KeyValuePair<string, string>>());
                        }
                        var querystring = Web.CleanUrl(url, onlyKeepQueries: new string[] { "id=", "item" }).Replace(uri, "");
                        urls[domain].Add(new KeyValuePair<string, string>(uri, querystring));
                    }

                    //get all download rules for all domains found on the page
                    var downloadRules = new List<Query.Models.DownloadRule>();
                    if (urls.Keys.Count > 0)
                    {
                        downloadRules = Query.Domains.DownloadRules.GetForDomains(urls.Keys.ToArray());
                    }

                    //add all links for all domains to download queue
                    foreach (var domain in urls.Keys)
                    {
                        try
                        {
                            if (Query.Blacklists.Domains.Check(domain)) { continue; }

                            //filter URLs that pass the download rules
                            ValidateURLs(domain, downloadRules, urls, out var urlsChecked);

                            var dlinks = urls[domain].Select(a => a.Key + a.Value);
                            if (dlinks.Count() > 0)
                            {
                                var count = Query.Downloads.AddQueueItems(dlinks.ToArray(), domain, feed.feedId);
                                await Clients.Caller.SendAsync("feed", count,
                                        "<span>(" + i + " of " + len + " feeds) Found " + count + " new link(s) from " + 
                                        feed.title + ": <a href=\"https://" + domain + "\" target=\"_blank\">" + domain + "</a></span>"
                                    );
                            }
                            else
                            {
                                //await Clients.Caller.SendAsync("feed", 0, "<span>(" + i + " of " + len + " feeds) No new links from " + feed.title + ": <a href=\"https://" + feed.url + "\" target=\"_blank\">" + feed.url + "</a></span>");
                            }
                        }
                        catch(Exception ex)
                        {
                            await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace + "<br/>" +
                                domain + ", " + string.Join(",", urls[domain].Distinct().ToArray()));
                        }
                    }
                }
                Query.Feeds.UpdateLastChecked(feed.feedId);
            }
            await Clients.Caller.SendAsync("update", "Checked feeds.");
            await Clients.Caller.SendAsync("checked", 1, 0, 0, 0, 0);
        }

        public async Task Whitelist(string domain)
        {
            Query.Whitelists.Domains.Add(domain);
            await Clients.Caller.SendAsync("update", "Whitelisted <a href=\"" + domain + "\" target=\"_blank\">" + domain + "</a> for the download queue");
        }

        public async Task Blacklist(string domain)
        {
            Query.Blacklists.Domains.Add(domain);
            try
            {
                //delete physical content for domain on disk
                Directory.Delete(App.MapPath("/Content/" + domain.Substring(0, 2) + "/" + domain), true);
            }
            catch (Exception)
            {
                await Clients.Caller.SendAsync("update", "Could not delete folder " + "/Content/" + domain.Substring(0, 2) + "/" + domain);
            }
            await Clients.Caller.SendAsync("update", "Blacklisted domain " + domain + " and removed all related articles");
        }

        private bool ValidateURL(string url)
        {
            if(url == "") { return false; }
            if (url.IndexOf("http://") != 0 && url.IndexOf("https://") != 0) { return false; }
            if (url.Length > 255) { return false; }
            if (Rules.badUrls.Any(a => url.Contains(a))) { return false; }
            if (!Domains.ValidateDomain(url.GetDomainName())) { return false; }
            return true;
        }

        private bool ValidateURLs(string domain, List<Query.Models.DownloadRule> downloadRules, Dictionary<string, List<KeyValuePair<string, string>>> urls, out List<string> urlsChecked) {
            var rules = downloadRules.Where(b => b.domain == domain);
            if (urls[domain] == null || urls[domain].Count == 0) { 
                urlsChecked = new List<string>();
                return false; 
            }

            //filter URLs that pass the download rules
            urlsChecked = urls[domain].Select(a => a.Key + a.Value)
                .Where(a =>
                {
                    var domainIndex = a.IndexOf(domain);
                    if (domainIndex != -1 && domainIndex + domain.Length + 1 <= a.Length)
                    {
                        var path = a.Substring(domainIndex + domain.Length + 1);
                        if (path == "")
                        {
                            return true;
                        }
                        return !rules.Any(b => b.rule == false &&
                        Domains.CheckDefaultDoNotDownloadRules(b.url, "", "") &&
                        Domains.CheckDownloadRule(b.url, "", "", path, "", ""));
                    }
                    return false;
                }
            ).Distinct().ToList();
            return true;
        }
    }
}
