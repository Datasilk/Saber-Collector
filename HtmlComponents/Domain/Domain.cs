using System;
using System.Collections.Generic;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.Articles
{
    public class Domain : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-domain",
                    Name = "Collector Domain",
                    Description = "View details about a domain",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/Domain/domain.css", "collector_domain_css");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/Domain/domain.js", "collector_domain_js");

                        results.Add(new KeyValuePair<string, string>(prefix + key, ""));
                        return results;
                    })
                }
            };
        }
    }
}
