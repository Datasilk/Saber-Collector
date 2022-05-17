using System;
using System.IO;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorArticles : Service, IVendorService
    {
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
