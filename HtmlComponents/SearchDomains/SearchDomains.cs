using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.UI
{
    public class SearchDomains : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "search-domains",
                    Name = "Search Domains",
                    Description = "Display a search form used to filter the results for the Collector Domains component",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();
                        var lang = request.Parameters.ContainsKey("lang") ? request.Parameters["lang"] : args.ContainsKey("lang") ? args["lang"] : "en";
                        var domaintype = int.Parse(request.Parameters.ContainsKey("type") ? request.Parameters["type"] : args.ContainsKey("type") ? args["type"] : "-1");

                        //render dropdown list of Feeds
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/SearchDomains/htmlcomponent.html");
                        viewComponent["subjects"] = Collector.Subjects.NavigateDropdown(0, false);
                        viewComponent["domain-types"] = "<option value=\"-1\">All Types</option>" +
                            string.Join("\n", Collector.Domains.TypesOrdered.Select(a =>
                                "<option value=\"" + a.Key + "\"" + (a.Key == domaintype ? " selected" : "") + ">" + a.Value + "</option>"
                            ).ToArray());
                        viewComponent["languages"] = "<option value=\"\">All Languages</option>" + 
                            string.Join("\n", Languages.KnownLanguages.Select(a => 
                                "<option value=\"" + a.Key + "\"" + (a.Key == lang ? " selected" : "") + ">" + a.Value + "</option>"
                            ).ToArray());
                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
