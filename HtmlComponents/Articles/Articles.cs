using System;
using System.Collections.Generic;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.Articles
{
    public class Articles : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-articles",
                    Name = "Collector Articles",
                    Description = "View a filtered list of articles in various ways",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render articles list
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/Articles/htmlcomponent.html");
                        var viewArticle = new View("/Vendors/Collector/HtmlComponents/Articles/list-item.html");
                        var html = new StringBuilder();

                        viewComponent["content"] = Components.Accordion.Render("Articles", "", Collector.Articles.RenderList(orderBy: Query.Articles.SortBy.highestScore));

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/Articles/articles.css", "collector_articles_css");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/Articles/articles.js", "collector_articles_js");

                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
