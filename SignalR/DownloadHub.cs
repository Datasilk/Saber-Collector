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
        private static int downloadsArchived = 0;

        public async Task CheckQueue(string id, int feedId, string domainName, int sort)
        {
            try
            {
                await Clients.Caller.SendAsync("update", "Checking queue...");
                var queue = Query.Downloads.CheckQueue(feedId, domainName, 60, (Query.Downloads.QueueSort)sort); // 60 second delay for each download on a single domain
                if (queue != null)
                {
                    if (CheckToStopQueue(id, Clients.Caller)) { return; }

                    if (!Domains.ValidateDomain(queue.domain))
                    {
                        //invalid domain, delete domain & all articles related to domain (shouldn't happen but just in case)
                        Domains.DeleteAllArticles(queue.domainId);
                        Query.Domains.IsDeleted(queue.domainId, true);
                        await Clients.Caller.SendAsync("update", "Invalid Domain");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        return;
                    }

                    if (!Domains.ValidateURL(queue.url))
                    {
                        //Delete download from database (shouldn't happen but just in case)
                        Query.Downloads.Delete(queue.qid);
                        await Clients.Caller.SendAsync("update", "Invalid URL");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        return;
                    }

                    //check download rules for queue
                    var downloadOnly = false;
                    foreach (var rule in queue.downloadRules)
                    {
                        if (rule.rule == false && rule.url != "" && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                        {
                            await Clients.Caller.SendAsync("update", "URL matches download rule \"" + rule.url + "\" and will be skipped (<a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>)");
                            await Clients.Caller.SendAsync("checked", 1, 0);
                            return;
                        }
                        else if (rule.rule == true && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                        {
                            downloadOnly = true;
                        }
                    }

                    AnalyzedArticle article = new AnalyzedArticle();

                    await Clients.Caller.SendAsync("update", "Downloading <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>...");

                    ///////////////////////////////////////////////////////////////////////////////////
                    //download content 
                    var result = "";
                    var newurl = "";
                    var isEmpty = false;
                    try
                    {
                        result = Article.Download(queue.url, out newurl);
                    }
                    catch(Exception ex)
                    {
                        downloadOnly = true;
                        if(sort == 2) { isEmpty = true; }
                    }
                    if (newurl != queue.url && newurl != "")
                    {
                        await Clients.Caller.SendAsync("update", "Redirected URL to <a href=\"" + newurl + "\" target=\"_blank\">" + newurl + "</a>");

                        ////////////////////////////////////////////////////////////////////////////////
                        //updated URL, retire current download and create new download for the new URL
                        Query.Downloads.Archive(queue.qid);
                        downloadsArchived++;
                        if(newurl.Length > 255)
                        {
                            await Clients.Caller.SendAsync("update", "Redirected URL is too long");
                            await Clients.Caller.SendAsync("checked", 1, 0);
                            return;
                        }
                        queue.qid = Query.Downloads.AddQueueItem(newurl, newurl.GetDomainName(), queue.parentId, feedId);
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
                                await Clients.Caller.SendAsync("checked", 1, 0);
                                return;
                            }
                            else if (rule.rule == true && Domains.CheckDownloadRule(rule.url, "", "", queue.url, "", "") == true)
                            {
                                downloadOnly = true;
                            }
                        }
                    }


                    ////////////////////////////////////////////////////////////////////////////////
                    // validate download results
                    await Clients.Caller.SendAsync("update", "Validating download...");
                    if (sort == 2)
                    {
                        //don't create articles for homepages
                        downloadOnly = true;
                    }

                    if (CheckToStopQueue(id, Clients.Caller)) { return; }
                    if (result == null || result == "")
                    {
                        //empty download result
                        await Clients.Caller.SendAsync("update", "Download timed out for URL: <a href=\"" + queue.url + "\" target=\"_blank\">" + queue.url + "</a>");
                        await Clients.Caller.SendAsync("checked", 0, 0);
                        if (sort == 2) { isEmpty = true; }
                    }
                    else if (result.Substring(0, 5) == "file:")
                    {
                        //was not HTML content
                        await Clients.Caller.SendAsync("update", "URL points to a file of type \"" + result.Substring(5) + "\"");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        return;
                    }
                    
                    if(result.StartsWith("\"Uncaught TypeError") || result.StartsWith("Object reference not set to an instance of an object"))
                    {
                        //Charlotte returned an error or timed out
                        await Clients.Caller.SendAsync("update", "Error parsing DOM!");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        //if (sort == 2) { isEmpty = true; }
                    }
                    else if(result.StartsWith("log: "))
                    {
                        //Charlotte returned an error or timed out
                        await Clients.Caller.SendAsync("update", "Request timeout!");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        if (sort == 2) { isEmpty = true; }
                    }
                    else if(isEmpty == false)
                    {
                        try
                        {
                            article = Html.DeserializeArticle(result);
                            article.feedId = queue.feedId;
                        }
                        catch (Exception ex)
                        {
                            await Clients.Caller.SendAsync("update", "Error parsing DOM!");
                            await Clients.Caller.SendAsync("checked", 1, 0);
                            //if (sort == 2) { isEmpty = true; }
                        }
                    }

                    if (article.url.StartsWith("chrome-error"))
                    {
                        //Charlotte returned a chrome-error:// as the resulting URL inside the JSON result
                        isEmpty = true;
                    }else if(article.elements.Count < 20)
                    {
                        isEmpty = true;
                    }

                    Query.Models.Article existingArticle = new Query.Models.Article();
                    Query.Models.Article articleInfo = new Query.Models.Article();
                    var isHomePage = false;

                    if(isEmpty == false)
                    {
                        //merge analyzed article into Query Article
                        existingArticle = Query.Articles.GetByUrl(queue.url) ?? Article.Create(queue.url);
                        articleInfo = Article.Merge(existingArticle, article);
                        
                        //check if URL is homepage
                        isHomePage = articleInfo.url.Length >= queue.domain.Length + 7 &&
                                articleInfo.url.Substring(articleInfo.url.IndexOf(queue.domain) + queue.domain.Length).Length <= 2;
                        
                        //validate title & summary
                        var checkTitleSummary = (articleInfo.title.ToLower() + " " + articleInfo.summary.ToLower()).Trim();
                        var checkTitle = articleInfo.title.ToLower();
                        if (isHomePage && checkTitleSummary != "" && 
                            (Rules.badHomePageTitles.Any(a => checkTitleSummary.IndexOf(a) >= 0) ||
                             Rules.badHomePageTitlesStartWith.Any(a => checkTitle.StartsWith(a))
                            ))
                        {
                            isEmpty = true;
                        }
                    }

                    if (isEmpty)
                    {
                        //domain doesn't contain any content //////////////////////////////
                        if (queue.articles == 0) { Query.Domains.IsEmpty(queue.domainId, true); }
                        await Clients.Caller.SendAsync("update", "Domain is empty");
                        await Clients.Caller.SendAsync("checked", 1, 0);
                        return;
                    }
                    else if(sort == 2)
                    {
                        Query.Domains.UpdateHttpsWww(queue.domainId, queue.url.Contains("https://"), queue.url.Contains("www."));
                    }

                    //process article /////////////////////////////////////////////////////
                    if (CheckToStopQueue(id, Clients.Caller)) { return; }
                    try
                    {
                        if(isHomePage)
                        {
                            //found home page
                            var lang = "";
                            if (articleInfo.summary != "")
                            {
                                lang = Languages.Detector.Detect(articleInfo.summary);
                            }
                            if(lang == null || lang != "en")
                            {
                                if(lang.IndexOf("cn") < 0 && lang.IndexOf("zh") < 0 && lang != "ja")
                                {
                                    //check body text to find dominant language
                                    var matchingLangs = new Dictionary<string, int>();
                                    var detected = "";
                                    foreach(var text in article.elements.Where(a => a.text != null).Select(a => a.text.Trim()))
                                    {
                                        detected = Languages.Detector.Detect(text);
                                        if(detected == null || detected == "") { continue; }
                                        if (matchingLangs.ContainsKey(detected))
                                        {
                                            matchingLangs[detected]+=text.Length;
                                        }
                                        else
                                        {
                                            matchingLangs.Add(detected, text.Length);
                                        }
                                    }
                                    var bestLang = "";
                                    var maxText = 0;
                                    foreach(var key in matchingLangs.Keys)
                                    {
                                        if(key == "") { continue; }
                                        if (matchingLangs[key] > maxText)
                                        {
                                            maxText = matchingLangs[key];
                                            bestLang = key;
                                        }
                                    }
                                    lang = bestLang;
                                }
                                if(lang != "en")
                                {
                                    Query.Domains.UpdateInfo(queue.domainId, articleInfo.title, articleInfo.summary, lang);
                                    await Clients.Caller.SendAsync("update", "Content is not in English");
                                    await Clients.Caller.SendAsync("checked", 1, 0);
                                    return;
                                }
                            }
                            Query.Domains.UpdateInfo(queue.domainId, articleInfo.title, articleInfo.summary, lang);
                        }

                        //check all download rules against article info
                        if (downloadOnly == false)
                        {
                            //check page title for phrases that will flag the url as empty
                            if ((articleInfo.title != "" && Rules.badPageTitles.Any(a => articleInfo.title.IndexOf(a) >= 0)) ||
                                (articleInfo.summary != "" && Rules.badPageTitles.Any(a => articleInfo.summary.IndexOf(a) >= 0)))
                            {
                                downloadOnly = true;
                            }

                            //check default download rules
                            if (Domains.CheckDefaultDownloadLinksOnlyRules(queue.url, articleInfo.title, articleInfo.summary))
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
                        }
                        
                        //get article score
                        Article.DetermineScore(article, articleInfo);

                        //get page score
                        if(downloadOnly == false)
                        {
                            var pageScore = Article.DeterminePageScore(article);
                            if(pageScore <= 40)
                            {
                                //do not save such a low-scoring download as an article
                                downloadOnly = true;
                            }
                        }
                        else
                        {

                            await Clients.Caller.SendAsync("update", "<span>" +
                                    "words: " + articleInfo.wordcount + ", sentences: " + articleInfo.sentencecount + ", important: " + articleInfo.importantcount + ", score: " + articleInfo.score +
                                    "(" + (article.subjects.Count > 0 ? string.Join(", ", article.subjects.Select(a => a.title)) : "") + ") " +
                                "</span>");
                        }

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
                                Query.Downloads.Archive(queue.qid);
                                downloadsArchived++;
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
                            Query.Downloads.Archive(queue.qid);
                            downloadsArchived++;
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
                        var blacklist = Query.Blacklists.Domains.CheckAll(keys);
                        for (var x = 0; x < keys.Length; x++)
                        {
                            var domain = keys[x];
                            try
                            {
                                if (CheckToStopQueue(id, Clients.Caller)) { return; }
                                if (blacklist.Any(a => a.domain == domain)) { continue; }
                                var rules = downloadRules.Where(b => b.domain == domain);
                                if (urls[domain] == null || urls[domain].Count == 0) { continue; }

                                //filter URLs that pass the download rules
                                ValidateURLs(domain, downloadRules, urls, out var urlsChecked);

                                //add filtered URLs to download queue
                                if(urlsChecked != null && urlsChecked.Count > 0)
                                {
                                    var count = Query.Downloads.AddQueueItems(urlsChecked.ToArray(), domain, queue.domainId, queue.feedId);
                                    if (count > 0)
                                    {
                                        addedLinks += count;
                                        await Clients.Caller.SendAsync("update",
                                            "<span>Found " + count + " new link(s) for <a href=\"https://" + domain + "\" target=\"_blank\">" + domain + "</a></span>");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                //ERROR !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                                await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace + "<br/>" +
                                    domain + ", " + string.Join(",", urls[domain].Distinct().ToArray()));
                            }
                        }

                        //finished processing download
                        await Clients.Caller.SendAsync("checked", 0, 1, downloadOnly == false && articleInfo.wordcount > 50 ? 1 : 0, 
                            addedLinks, downloadOnly == false ? articleInfo.wordcount ?? 0 : 0, 
                            downloadOnly == false ? articleInfo.importantcount ?? 0 : 0);

                        //check if we should move archived downloads
                        if (downloadsArchived > 1000)
                        {
                            downloadsArchived = 0;
                            await Clients.Caller.SendAsync("update", "Archiving the last 1,000 completed downloads");
                            Query.Downloads.MoveArchived(); 

                        }
                        
                        //destroy references
                        article = null; 
                        articleInfo = null;
                        existingArticle = null;
                        
                        return;
                    }
                    catch (Exception ex)
                    {
                        //ERROR !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                        await Clients.Caller.SendAsync("update", "Error: " + ex.Message + "<br/>" + ex.StackTrace);
                        await Clients.Caller.SendAsync("checked", 1, 0, 0, 0, 0, 0);
                        Query.Logs.LogError(0, queue.url, "DownloadHub", ex.Message, ex.StackTrace);
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
                if(ex.Message.Contains("No connection could be made because the target machine actively refused it"))
                {
                    //No connection could be made because the target machine (Charlotte's Web Router) actively refused it. (localhost:7007)
                    await Clients.Caller.SendAsync("update", ex.Message);
                }
                else
                {
                    //ERROR !!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    await Clients.Caller.SendAsync("update", "An error occurred while checking the download queue");
                    await Clients.Caller.SendAsync("checked", 1, 0, 0, 0, 0, 0);
                }
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
            if(len == 0)
            {
                await Clients.Caller.SendAsync("update", "Checked feeds.");
                await Clients.Caller.SendAsync("checkedfeeds");
                return;
            }
            foreach (var feed in feeds)
            {
                i++;

                //update database (if multiple download instances running)
                Query.Feeds.UpdateLastChecked(feed.feedId);

                //read feed (either RSS or HTML)
                if (feed.doctype == Query.Feeds.DocTypes.RSS)
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
                                count += Query.Downloads.AddQueueItems(dlinks.ToArray(), domain, feed.domainId, feed.feedId);
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
                        //Console.WriteLine(ex.Message);
                        //Console.WriteLine(ex.StackTrace);
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
                    var totalQueueItems = 0;
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
                                totalQueueItems += Query.Downloads.AddQueueItems(dlinks.ToArray(), domain, feed.domainId, feed.feedId);
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
                    await Clients.Caller.SendAsync("feed", totalQueueItems,
                        "<span>(" + len + " feeds left) Found " + totalQueueItems + " new link(s) from " + 
                        urls.Keys.Count + " domains for feed: <a href=\"" + feed.url + "\" target=\"_blank\">" + feed.url + "</a></span>"
                    );
                }

                //recursively check feeds
                if(feeds.Count == 1)
                {
                    await Clients.Caller.SendAsync("update", "Checked feeds.");
                    await Clients.Caller.SendAsync("checkedfeeds");
                }
                else
                {
                    await CheckFeeds(feedId);
                }
                return;
            }
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
            var domain = url.GetDomainName();
            if (Rules.badUrls.Any(a => url.Contains(a))) { return false; }
            if (Rules.badUrlExtensions.Any(a => url.Contains("." + a))) { return false; }
            if (!Domains.ValidateDomain(domain)) { return false; }
            return true;
        }

        private bool ValidateURLs(string domain, List<Query.Models.DownloadRule> downloadRules, Dictionary<string, List<KeyValuePair<string, string>>> urls, out List<string> urlsChecked) {
            //check blacklist wildcards
            foreach(var wildcard in Collector.Blacklist.Wildcards)
            {
                if (wildcard.Match(domain).Success)
                {
                    urlsChecked = new List<string>();
                    return false;
                }
            }

            //check for numerical root domain
            var domainparts = domain.Split(".");
            if(domainparts.Skip(1).Any(a => a[0].Asc() >= 48 && a[0].Asc() <= 57))
            {
                //numerical root domain detected
                urlsChecked = new List<string>();
                return false;
            }
            
            //check download rules for domain
            var rules = downloadRules.Where(b => b.domain == domain);
            if (urls[domain] == null || urls[domain].Count == 0) { 
                urlsChecked = new List<string>();
                return false; 
            }

            //filter URLs that pass the download rules
            urlsChecked = urls[domain].Select(a => a.Key + a.Value)
                .Where(a =>
                {
                    if(a.Length > 255) { return false; }
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
