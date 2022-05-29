using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorDomains : Service, IVendorService
    {

        public string Search(int subjectId, int type, int sort, string search, int start, int length)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            return Domains.RenderList(subjectId, (Domains.Type)type, (Domains.Sort)sort, start, length, search);
        }
    }
}
