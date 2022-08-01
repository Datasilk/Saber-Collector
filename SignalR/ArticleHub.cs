using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Saber.Vendors.Collector.Models.Article;
using Saber.Core.Extensions.Strings;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Hubs
{
    public class ArticleHub : Hub
    {
        private IVendorInfo info = new Info();

        public async Task AnalyzeArticle(string url, bool published)
        {
            
            await Clients.Caller.SendAsync("update", 1, "Collector v" + info.Version);

            //first, validate URL
            if (!Domains.ValidateURL(url))
            {
                await Clients.Caller.SendAsync("update", 1, "URL \"" + url + "\" is invalid");
                return;
            }

            // Get Article HTML Content //////////////////////////////////////////////////////////////////////////////////////////////////
            try
            {
                var download = true;
                AnalyzedArticle article = new AnalyzedArticle();
                var articleInfo = Query.Articles.GetByUrl(url);

                if (articleInfo != null)
                {
                    //article exists in database
                    if (!published)
                    {
                        await Clients.Caller.SendAsync("update", 1, "Article exists in database");
                    }
                }


                var relpath = Article.ContentPath(url);
                var filepath = App.MapPath(relpath);
                var filename = "";
                if(articleInfo != null && articleInfo.articleId > 0)
                {
                    filename = articleInfo.articleId + ".html";
                    if (File.Exists(filepath + filename))
                    {
                        //open cached content from disk
                        article = Html.DeserializeArticle(File.ReadAllText(filepath + filename));
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "Loaded cached content for URL: <a href=\"" + url + "\" target=\"_blank\">" + url + "</a>");
                            await Clients.Caller.SendAsync("update", 1, "Cached file located at: " + filepath + filename + " (" +
                                "<a href=\"javascript:\" onclick=\"article.delete(" + articleInfo.articleId + ")\">delete</a>)");
                        }
                        
                        download = false;
                        Article.FileSize(article);
                    }
                }

                if (download == true)
                {
                    //download article from the internet
                    if (!published)
                    {
                        await Clients.Caller.SendAsync("update", 1, "Downloading...");
                    }
                    var result = Article.Download(url, out var newurl);

                    if (result == "")
                    {
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "Download timed out for URL: <a href=\"" + url + "\" target=\"_blank\">" + url + "</a>");
                        }
                        return;
                    }
                    else if (result.IndexOf("file:") == 0)
                    {
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "URL points to a file of type \"" + result.Substring(5) + "\"");
                            await Clients.Caller.SendAsync("update", 1, "Download file: <a href=\"" + url + "\" target=\"_blank\">" + url + "</a>");
                        }
                        return;
                    }
                    try
                    {
                        article = Html.DeserializeArticle(result);
                        if(article.url != null && article.url != "")
                        {
                            //get URL redirection from Charlotte
                            newurl = article.url;
                        }
                    }
                    catch (Exception)
                    {
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "Error parsing DOM!");
                            await Clients.Caller.SendAsync("update", 1, result.Replace("&", "&amp;").Replace("<", "&lt;").Replace("\n", "<br/>"));
                        }
                        return;
                    }

                    if (newurl != url)
                    {
                        //updated URL (was redirected)
                        if(articleInfo != null)
                        {
                            //update existing article URL
                            Query.Articles.UpdateUrl(articleInfo.articleId, newurl, newurl.GetDomainName());
                            await Clients.Caller.SendAsync("update", 1, "Redirected & updated article URL from " + url + " to " + newurl);
                        }
                        await Clients.Caller.SendAsync("update-url", newurl);

                        if(articleInfo == null)
                        {
                            var oldinfo = articleInfo;
                            articleInfo = Query.Articles.GetByUrl(newurl);
                            if(articleInfo != null)
                            {
                                filename = articleInfo.articleId + ".html";
                                if (oldinfo != null && oldinfo.articleId != articleInfo.articleId)
                                {
                                    await Clients.Caller.SendAsync("update", 1, "Old articleId = " + oldinfo.articleId + ", new articleId = " + articleInfo.articleId);
                                }
                                await Clients.Caller.SendAsync("update", 1, "Redirected & updated article URL from " + url + " to " + newurl);
                            }
                        }
                    }
                    
                    if(articleInfo == null)
                    {
                        //no article yet
                        articleInfo = Article.Add(newurl);
                        filename = articleInfo.articleId + ".html";
                        await Clients.Caller.SendAsync("update", 1, "Created new record for article");
                    }

                    url = newurl;

                    relpath = Article.ContentPath(url);
                    filepath = App.MapPath(relpath);


                    //get filesize of article
                    Article.FileSize(article);

                    if (article.rawHtml.Length == 0)
                    {
                        //article HTML is empty
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "URL returned an empty response");
                        }
                        return;
                    }

                    if (!Directory.Exists(filepath))
                    {
                        //create folder for content
                        Directory.CreateDirectory(filepath);
                    }

                    File.WriteAllText(filepath + filename, result);
                    Query.Articles.UpdateCache(articleInfo.articleId, true);

                    if (!published)
                    {
                        await Clients.Caller.SendAsync("update", 1, "Downloaded URL (" + article.fileSize + " KB" + "): <a href=\"" + url + "\" target=\"_blank\">" + url + "</a>");
                        await Clients.Caller.SendAsync("update", 1, "Cached file located at: " + filepath + filename + " (" +
                            "<a href=\"javascript:\" onclick=\"article.delete(" + articleInfo.articleId + ")\">delete</a>)");
                    }
                    
                }

                //set article information
                article.url = url;
                article.id = articleInfo.articleId;
                article.feedId = articleInfo.feedId ?? -1;
                article.domain = Web.GetDomainName(url);
                Html.GetArticleInfoFromDOM(article);

                if (!published)
                {
                    await Clients.Caller.SendAsync("update", 1, "Parsed DOM tree (" + article.elements.Count + " elements)");
                    await Clients.Caller.SendAsync("update", 1, "Analyzing DOM...");
                }

                //find elements to protect/exclude based on domain-level analyzer rules
                var rules = Query.Domains.AnalyzerRules.GetList(articleInfo.domainId);
                var protectedIndexes = new List<int>();
                var excludedIndexes = new List<int>();
                var indexFlagInfo = new Dictionary<int, Query.Models.AnalyzerRule>();

                foreach (var rule in rules)
                {
                    if(rule.rule == false)
                    {
                        //remove elements from article
                        var elems = Html.FindElements(rule.selector, article.elements, article.elements);
                        foreach(var elem in elems)
                        {
                            excludedIndexes.Add(elem.index);
                            if (!indexFlagInfo.ContainsKey(elem.index))
                            {
                                indexFlagInfo.Add(elem.index, rule);
                            }
                        }
                    }
                    else
                    {
                        //protect elements in article
                        var elems = Html.FindElements(rule.selector, article.elements, article.elements);
                        foreach (var elem in elems)
                        {
                            protectedIndexes.Add(elem.index);
                            if (!indexFlagInfo.ContainsKey(elem.index))
                            {
                                indexFlagInfo.Add(elem.index, rule);
                            }
                        }
                    }
                }

                //get article elements
                var elements = new List<AnalyzedElement>();
                Html.GetBestElementIndexes(article, elements);

                //mark best elements as protected or bad based on analyzer rules
                foreach(var elem in elements)
                {
                    if (excludedIndexes.Contains(elem.index))
                    {
                        elem.isBad = true;
                        elem.flags.Add(ElementFlags.ExcludedAnalyzerRule);
                        elem.flagInfo = indexFlagInfo[elem.index] != null ? indexFlagInfo[elem.index].selector : "";
                        //reset all child elements
                        foreach (var elem2 in elements)
                        {
                            if (elem2.Element.hierarchyIndexes.Contains(elem.index))
                            {
                                elem2.flags = new List<ElementFlags>();
                                elem2.isBad = true;
                                elem2.badClasses = 0;
                                elem2.badClassNames = new List<string>();
                                elem2.isContaminated = true;
                                elem2.wordsInHierarchy = 0;
                            }
                        }
                    }
                    else if (protectedIndexes.Contains(elem.index))
                    {
                        elem.isBad = false;
                        elem.isContaminated = false;
                        elem.flags.Clear();
                        elem.flags.Add(ElementFlags.ProtectedAnalyzerRule);
                        elem.flagInfo = indexFlagInfo[elem.index] != null ? indexFlagInfo[elem.index].selector : "";
                        elem.badClasses = 0;
                        elem.badClassNames.Clear();
                        elem.counters.Clear();
                    }
                }

                //add any missing elements that are protected
                foreach(var index in protectedIndexes.Where(a => !elements.Any(b => b.index == a)))
                {
                    elements.Add(new AnalyzedElement()
                    {
                        index = index,
                        Element = article.elements.Where(a => a.index == index).FirstOrDefault(),
                        flags = new List<ElementFlags>()
                        {
                            ElementFlags.ProtectedAnalyzerRule
                        }
                    });
                }

                //make sure elements are in the correct order
                elements = elements.OrderBy(a => a.index).ToList();

                //finally, get article text & images based on best DOM element indexes
                Html.GetArticleElements(article, elements);
                if (!published)
                {
                    await Clients.Caller.SendAsync("update", 1, "Collected article contents from DOM");
                }

                //send accordion with raw HTML to client
                var rawhtml = Article.RenderRawHTML(article, elements);
                var html = Components.Accordion.Render("Raw HTML", "raw-html", "<div class=\"empty-top\"></div><div class=\"empty-bottom\"></div>", false);
                await Clients.Caller.SendAsync("append", html);
                if (!published)
                {
                    await Clients.Caller.SendAsync("rawhtml", rawhtml);
                    await Clients.Caller.SendAsync("update", 1, "Generated Raw HTML for dissecting DOM importance");
                }
                
                var imgCount = 0;
                var imgTotalSize = 0;

                if(article.body.Count > 0)
                {
                    //found article content
                    if (!published)
                    {
                        await Clients.Caller.SendAsync("update", 1, "Found article text...");
                    }
                        
                    Html.GetImages(article);
                    
                    if(article.images.Count > 0)
                    {
                        //images exist, download related images for article
                        if (!published)
                        {
                            await Clients.Caller.SendAsync("update", 1, "Downloading images for article...");
                        }
                            
                        //build image path within wwwroot folder
                        var imgpath = "/wwwroot/" + relpath.ToLower() + "/"; // + article.id + "/";

                        //check if img folder exists
                        if (!Directory.Exists(App.MapPath(imgpath)))
                        {
                            Directory.CreateDirectory(App.MapPath(imgpath));
                        }

                        var cachedCount = 0;

                        for(var x = 0; x < article.images.Count; x++)
                        {
                            //download each image
                            var img = article.images[x];
                            var path = App.MapPath(imgpath + img.filename);
                            if (!File.Exists(path))
                            {
                                try
                                {
                                    using (WebClient webClient = new WebClient())
                                    {
                                        webClient.DownloadFile(new Uri(img.url), path);
                                        var filesize = File.ReadAllBytes(path).Length / 1024;
                                        imgCount++;
                                        imgTotalSize += filesize;
                                        if (!published)
                                        {
                                            await Clients.Caller.SendAsync("update", 1, "Downloaded image \"" + img.filename + "\" (" + filesize + " kb)");
                                        } 
                                    }
                                }catch(Exception)
                                {
                                    if (!published)
                                    {
                                        await Clients.Caller.SendAsync("update", 1, "Image Download Error: \"" + img.filename + "\"");
                                    }
                                }
                            }
                            else
                            {
                                cachedCount++;
                                imgCount++;
                            }
                            if (File.Exists(path))
                            {
                                var filesize = File.ReadAllBytes(path).Length / 1024;
                                imgTotalSize += filesize;
                                article.images[x].exists = true;
                            }
                        }

                        if(cachedCount > 0)
                        {
                            if (!published)
                            {
                                await Clients.Caller.SendAsync("update", 1, cachedCount + " images have already been cached on the server");
                            }
                        }
                    }

                    //render article
                    html = Components.Accordion.Render("Article Text", "article-text", Article.RenderArticle(article), false);
                    await Clients.Caller.SendAsync("append", html);
                }

                if (!published)
                {
                    //display list of words found
                    var allwords = Html.GetWordsOnly(article);
                    var uniqueWords = allwords.Where(a => a.Length > 1 && a.Substring(0, 1).IsNumeric() == false && 
                        !Rules.commonWords.Contains(a.ToLower())).Select(a => a.ToLower()).Distinct().OrderBy(a => a).ToList();
                    var subjectWords = Query.Words.GetList(uniqueWords.ToArray());
                    html = Components.Accordion.Render("Words", "article-words", Article.RenderWordsList(article, uniqueWords, subjectWords), false);
                    await Clients.Caller.SendAsync("words", html);

                    //display list of phrases found
                    try
                    {
                        html = Components.Accordion.Render("Phrases", "article-phrases", Article.RenderPhraseList(article), false);
                        await Clients.Caller.SendAsync("phrases", html);
                    }
                    catch (Exception ex)
                    {
                        await Clients.Caller.SendAsync("update", 1, ex.Message);
                    }

                    //update article info in database
                    await Clients.Caller.SendAsync("update", 1, "Updating database records...");

                    articleInfo.title = article.title;
                    articleInfo.analyzecount++;
                    articleInfo.analyzed = Article.Version;
                    articleInfo.cached = true;
                    articleInfo.domain = article.domain;
                    articleInfo.feedId = article.feedId;
                    articleInfo.summary = article.summary;
                    articleInfo.fiction = (short)(article.fiction == true ? 1 : 0);
                    articleInfo.filesize = article.fileSize + imgTotalSize;
                    articleInfo.images = Convert.ToByte(imgCount);
                    articleInfo.importance = (short)article.importance;
                    articleInfo.importantcount = (short)article.totalImportantWords;
                    articleInfo.paragraphcount = (short)article.totalParagraphs;
                    articleInfo.relavance = (short)article.relevance;
                    try
                    {
                        var subj = article.subjects.OrderBy(a => a.score * -1).First();
                        if(subj != null)
                        {
                            articleInfo.score = (short)subj.score;
                            articleInfo.subjectId = subj.id;
                            articleInfo.subjects = Convert.ToByte(article.subjects.Count);
                        }
                    }
                    catch (Exception) { }
                    articleInfo.yearstart = (short)article.yearStart;
                    articleInfo.yearend = (short)article.yearEnd;
                    try
                    {
                        articleInfo.years = string.Join(",", article.years.ToArray());
                    }
                    catch (Exception) { }

                    var text = article.rawText.Replace("\n", "").Replace("\r", "");
                    var words = Html.CleanWords(Html.SeparateWordsFromText(text));
                    article.totalWords = words.Length;
                    article.totalSentences = Html.GetSentences(text).Count;
                    article.totalImportantWords = subjectWords.Count;

                    articleInfo.sentencecount = (short)article.totalSentences;
                    articleInfo.wordcount = article.totalWords;
                    await Clients.Caller.SendAsync("update", 1, "Word Count: " + articleInfo.wordcount);
                    await Clients.Caller.SendAsync("update", 1, "Important Words: " + articleInfo.importantcount);

                    //get article score
                    var scoreInfo = Article.DetermineScore(article, articleInfo);
                    await Clients.Caller.SendAsync("update", 1, "Article Score: <b>" + scoreInfo.score + "</b> of 100 possible points &nbsp;&nbsp;&nbsp;&nbsp;(" + Math.Round(scoreInfo.quality, 1) + "% \"quality content\"" +
                        " / " + articleInfo.wordcount + " \"total words\" * " + scoreInfo.linkWordCount + " \"link words\") = " + (Math.Round(scoreInfo.linkRatio, 1) + "% \"link ratio\"") + ")");

                    //update article info in database
                    Query.Articles.Update(articleInfo);
                }

                //increment article visited
                Query.Articles.Visited(articleInfo.articleId);

                //finished
                if (!published)
                {
                    await Clients.Caller.SendAsync("update", 1, "Done!");
                    await Clients.Caller.SendAsync("update", 1, "<a href=\"?url=" + WebUtility.UrlEncode(url) + "&article-only=1\" target=\"_blank\">View Article</a>");
                }
            }
            catch (Exception ex)
            {
                if (!published)
                {
                    await Clients.Caller.SendAsync("update", 1, "Error: " + ex.Message + "<br/>" + ex.StackTrace.Replace("\n", "<br/>"));
                }
            }
        }
    }
}
