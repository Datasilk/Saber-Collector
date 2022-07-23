using System;
using System.Collections.Generic;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.UI.ContentFields.DomainCollections
{
    [ContentField("domain-collections")]
    public class ContentField : IVendorContentField
    {
        public string Render(IRequest request, Dictionary<string, string> args, string data, string id, string prefix, string key, string lang, string container)
        {
            var viewfield = new View("/Vendors/Collector/HtmlComponents/DomainCollections/contentfield.html");
            return viewfield.Render();
        }
    }
}
