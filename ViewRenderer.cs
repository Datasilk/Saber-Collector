using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector
{
    [ViewPath("/Views/Shared/layout.html")]
    public class ViewRenderer : IVendorViewRenderer
    {
        public string Render(IRequest request, View view)
        {
            view["body-class"] += " box";
            view.Show("has-body-class");
            return "";
        }
    }
}