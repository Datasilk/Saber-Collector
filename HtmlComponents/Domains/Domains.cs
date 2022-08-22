using System;
using System.Collections.Generic;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.Articles
{
    public class Domains : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-domains",
                    Name = "Collector Domains",
                    Description = "View a filtered list of domains that Collector has touched",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/Domains/domains.css", "collector_domains_css");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/Domains/domains.js", "collector_domains_js");

                        //results.Add(new KeyValuePair<string, string>(prefix + key, Collector.Domains.RenderComponent()));
                        return results;
                    })
                }
            };
        }
    }
}
