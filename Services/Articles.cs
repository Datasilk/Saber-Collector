using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorArticles : Service, IVendorService
    {

        public string Search(int subjectId, int feedId, int score, int sort, string search, int start, int length)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Articles.RenderList(subjectId, feedId, start, length, score, search, Query.Articles.IsActive.Both, false, 0, null, null, (Query.Articles.SortBy)sort);
        }

        public string DeleteJsonCachedHtmlFile(int articleId)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var articleInfo = Query.Articles.GetById(articleId);
            var relpath = Article.ContentPath(articleInfo.url);
            var filepath = App.MapPath(relpath);
            var filename = articleId + ".html";
            if(File.Exists(filepath + filename))
            {
                try
                {
                    File.Delete(filepath + filename);
                    Query.Articles.UpdateCache(articleId, false);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message + "\n" + ex.StackTrace);
                    return Error("Could not delete file " + filename);
                }
                
            }
            return Success();
        }
    }
}
