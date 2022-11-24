using System;
using System.Collections.Generic;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.UI
{
    public class DownloadsDropdown : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "downloads-dropdown",
                    Name = "Downloads Dropdown",
                    Description = "Display a dropdown list of settings related to Collector's article download system",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render dropdown list of menu items
                        var viewComponent = new View("/Vendors/Collector/HtmlComponents/DownloadsDropdown/htmlcomponent.html");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/downloadsdropdown/downloads.js", "downloads_dropdown_js");
                        results.Add(new KeyValuePair<string, string>(prefix + key, viewComponent.Render()));
                        return results;
                    })
                }
            };
        }
    }
}