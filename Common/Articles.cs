using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Saber.Vendors.Collector
{
    public static class Articles
    {
        public static string RenderList(int subjectId = 0, int feedId = 0, int domainId = 0, int start = 1, int length = 50, int score = 0, string search = "", Query.Articles.IsActive isActive = Query.Articles.IsActive.Both, bool isDeleted = false, int minImages = 0, DateTime? dateStart = null, DateTime? dateEnd = null, Query.Articles.SortBy orderBy = Query.Articles.SortBy.BestScore)
        {
            List<Query.Models.ArticleDetails> articles;
            var subjectIds = new List<int>();
            if(subjectId > 0)
            {
                subjectIds.Add(subjectId);
            }
            articles = Query.Articles.GetList(subjectIds.ToArray(), feedId, domainId, score, search, Query.Articles.IsActive.Both, false, minImages, dateStart, dateEnd, orderBy, start, length);

            var item = new View("/Vendors/Collector/HtmlComponents/Articles/list-item.html");
            var html = new StringBuilder();
            if (articles != null)
            {
                foreach (var article in articles)
                {
                    //populate view with article info
                    item.Clear();
                    item["title"] = article.title;
                    item["encoded-url"] = WebUtility.UrlEncode(article.url);
                    item["url"] = article.url;

                    if (article.score > 0)
                    {
                        //show score
                        item.Show("show-score");
                        item["score"] = string.Format("{0:N0}", article.score);
                    }


                    if (article.breadcrumb != null)
                    {
                        //show breadcrumb
                        item.Show("show-breadcrumb");
                        var bread = article.breadcrumb.Split('>');
                        var hier = article.hierarchy.Split('>');
                        var crumb = "";
                        var hasSubject = false;
                        for (var b = 0; b < bread.Length; b++)
                        {
                            crumb += (crumb != "" ? " > " : "") + "<a href=\"/subject/" + hier[b] + "\">" + bread[b] + "</a>";
                            if (int.Parse(hier[b]) == subjectId) { hasSubject = true; }
                        }
                        if (hasSubject == false)
                        {
                            crumb += (crumb != "" ? " > " : "") + "<a href=\"/subjects?id=" + subjectId + "\">" + article.subjectTitle + "</a>";
                        }
                        item["breadcrumb"] = crumb;
                    }

                    if (article.filesize != null && article.filesize > 0)
                    {
                        //show file size
                        item.Show("show-file-size");
                        item["file-size"] = Math.Round(article.filesize.Value, 2).ToString();
                    }

                    if (article.wordcount != null && article.wordcount > 0)
                    {
                        //show words
                        item.Show("show-words");
                        item["words"] = string.Format("{0:N0}", article.wordcount);
                    }

                    if (article.sentencecount != null && article.sentencecount > 0)
                    {
                        //show sentences
                        item.Show("show-sentences");
                        item["sentences"] = string.Format("{0:N0}", article.sentencecount);
                    }

                    if (article.importantcount != null && article.importantcount > 0)
                    {
                        //show important words
                        item.Show("show-important-words");
                        item["important-words"] = string.Format("{0:N0}", article.importantcount);
                    }

                    if (article.years != null && article.years != "")
                    {
                        //show words
                        item.Show("show-years");
                        item["years"] = article.years.Replace(",", ", ");
                    }

                    html.Append(item.Render());
                }
            }
            return html.ToString();
        }
    }
}
