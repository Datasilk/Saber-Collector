using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents
{
    public class Analyzer : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "article-analyzer",
                    Name = "Article Analyzer",
                    Description = "Collect & examine various types of data found when analyzing a remote URL",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>()
                    {
                        {"query-param", new HtmlComponentParameter()
                            {
                                Name = "URL Query Parameter",
                                Description = "Choose the name of a querystring parameter to use when passing the URL you wish to analyze",
                                DefaultValue = "url",
                                DataType = HtmlComponentParameterDataType.Text,
                                Required = true
                            } 
                        }
                    },
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render analyzer accordion
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/Analyzer/htmlcomponent.html");

                        viewComponent["content"] = Components.Accordion.Render(
                            "Analyze Article: " + (request.Parameters.ContainsKey("url") ? request.Parameters["url"] : "???"),
                            "analyze-article",
                            Cache.LoadFile("/Vendors/Collector/HtmlComponents/Analyzer/analyze.html")
                        );

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/analyzer/analyzer.css");
                        request.AddScript("/editor/js/utility/signalr/signalr.js");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/analyzer/analyzer.js");

                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
