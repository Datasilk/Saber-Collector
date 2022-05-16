using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.UI
{
    public class FeedDropdown : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "feed-dropdown",
                    Name = "Feeds Dropdown",
                    Description = "Display a list of feeds in a dropdown field",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(){
                        {"all-feeds", new HtmlComponentParameter()
                            {
                                Name = "Can select all feeds",
                                Description = "Include option to select all feeds",
                                DefaultValue = "1",
                                DataType = HtmlComponentParameterDataType.Boolean,
                                Required = false
                            }
                        }
                    },
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render dropdown list of Feeds
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/FeedDropdown/dropdown.html");
                        viewComponent["options"] = 
                            (args.ContainsKey("all-feeds") && args["all-feeds"] == "1" ? "<option value=\"0\">All Feeds</option>" : "") + 
                            string.Join("\n", Query.Feeds.GetList().Select(a => "<option value=\"" + a.feedId + "\">" + a.title + "</option>"));
                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
