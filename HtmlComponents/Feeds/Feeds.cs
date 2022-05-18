using System;
using System.Collections.Generic;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents
{
    public class Feeds : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-feeds",
                    Name = "Collector Feeds",
                    Description = "Manage a list of feeds that Collector will check periodically for new URLs",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render feeds list
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/Feeds/htmlcomponent.html");
                        var categories = Query.Feeds.GetCategories();
                        viewComponent["category-options"] = Collector.Feeds.RenderCategoryOptions(categories);
                        viewComponent["content"] = Collector.Feeds.RenderList(categories);
                        if(viewComponent["content"] == "")
                        {
                            viewComponent.Show("no-feeds");
                        }

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/feeds/feeds.css", "collector_feeds_css");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/feeds/feeds.js", "collector_feeds_js");

                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
