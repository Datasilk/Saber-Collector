using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.UI
{
    public class SearchArticles : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "search-articles",
                    Name = "Search Articles",
                    Description = "Display a search form used to filter the results for the Collector Articles component",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(){
                        {"no-subjects", new HtmlComponentParameter()
                            {
                                Name = "Hide subjects dropdown",
                                Description = "If user wants to specify a subject, use the subjectId query string parameter",
                                DataType = HtmlComponentParameterDataType.Boolean,
                                Required = false
                            }
                        }
                    },
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render dropdown list of Feeds
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/SearchArticles/htmlcomponent.html");
                        viewComponent["subjects"] = Collector.Subjects.NavigateDropdown(0, false);
                        viewComponent["feeds"] = viewComponent["options"] = ("<option value=\"0\" selected>All Feeds</option>") +
                            string.Join("\n", Query.Feeds.GetList().Select(a => "<option value=\"" + a.feedId + "\">" + a.title + "</option>"));
                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
