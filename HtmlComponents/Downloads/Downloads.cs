using System;
using System.Collections.Generic;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.Downloads
{
    public class Downloads : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-downloads",
                    Name = "Collector Downloads",
                    Description = "Actively download web pages and spider websites for new web pages to download",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render downloads list
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/Downloads/htmlcomponent.html");
                        var viewStatistics = new View("/Vendors/Collector/HtmlComponents/Downloads/statistics.html");
                        var viewStat = new View("/Vendors/Collector/HtmlComponents/Downloads/stat.html");
                        var html = new StringBuilder();
                        
                        //total downloads
                        viewStat.Clear();
                        viewStat.Show("number");
                        viewStat["id"] = "downloads";
                        viewStat["title"] = "Downloads";
                        html.Append(viewStat.Render());
                        
                        //total articles
                        viewStat.Clear();
                        viewStat.Show("number");
                        viewStat["id"] = "articles";
                        viewStat["title"] = "Articles";
                        html.Append(viewStat.Render());

                        //total links
                        viewStat.Clear();
                        viewStat.Show("number");
                        viewStat["id"] = "links";
                        viewStat["title"] = "New Links";
                        html.Append(viewStat.Render());

                        //total words
                        viewStat.Clear();
                        viewStat.Show("number");
                        viewStat["id"] = "words";
                        viewStat["title"] = "Words";
                        html.Append(viewStat.Render());

                        //total important
                        //viewStat.Clear();
                        //viewStat.Show("number");
                        //viewStat["id"] = "important";
                        //viewStat["title"] = "Important";
                        //html.Append(viewStat.Render());

                        viewStatistics["stats"] = html.ToString();


                        viewComponent["content"] = Components.Accordion.Render("Statistics", "", viewStatistics.Render()) +
                        Components.Accordion.Render("Downloads", "section-downloads hide", Cache.LoadFile("/Vendors/Collector/HtmlComponents/Downloads/console.html")) +
                        Components.Accordion.Render("Articles Found", "section-articles hide", Cache.LoadFile("/Vendors/Collector/HtmlComponents/Downloads/articles-found.html"));

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/downloads/downloads.css", "collector_downloads_css");
                        request.AddScript("/editor/js/utility/signalr/signalr.js", "collector_signalr");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/downloads/downloads.js", "collector_downloads_js");
                        if (request.Parameters.ContainsKey("run"))
                        {
                            //auto-run the download queue
                            request.AddScriptBlock("setTimeout(S.downloads.start, 1000);");
                        }
                        if (request.Parameters.ContainsKey("feedid"))
                        {
                            viewComponent.Show("has-feedid");
                            viewComponent["feedid"] = request.Parameters["feedid"];
                        }
                        if (request.Parameters.ContainsKey("domain"))
                        {
                            viewComponent.Show("has-domain");
                            viewComponent["domain"] = request.Parameters["domain"];
                        }
                        if (request.Parameters.ContainsKey("sort"))
                        {
                            viewComponent.Show("has-sort");
                            viewComponent["sort"] = request.Parameters["sort"];
                        }
                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
