using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Saber.Vendor;

namespace Saber.Vendors.Collector
{
    public class SignalR : IVendorSignalR
    {
        public void RegisterHubs(IEndpointRouteBuilder endpoints)
        {
            endpoints.MapHub<Hubs.ArticleHub>("/articlehub");
            endpoints.MapHub<Hubs.DownloadHub>("/downloadhub");
        }
    }
}
