using System;
using System.Linq;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.Services
{
    public class CollectorSubjects : Service, IVendorService
    {
        public string AddSubjects(string subjects, string hierarchy)
        {
            if (!CheckSecurity()) { return AccessDenied(); }
            var subjectList = subjects.Replace(" , ", ",").Replace(", ", ",").Replace(" ,", ",").Split(',');
            var hier = hierarchy != "" ? hierarchy.Replace(" > ", ">").Replace("> ", ">").Replace(" >", ">").Split('>') : new string[] { };
            try
            {
                Subjects.Add(subjectList, hier);
                
                var parentId = 0;
                if (hierarchy.Length > 0)
                {
                    var parentHier = hier.ToList();
                    var parentTitle = hier[hier.Length - 1];
                    parentHier.RemoveAt(parentHier.Count - 1);
                    var parentBreadcrumb = string.Join(">", parentHier);
                    var subject = Query.Subjects.GetSubjectByTitle(parentTitle, parentBreadcrumb);
                    parentId = subject.subjectId;
                }

                return JsonResponse(Subjects.RenderList(Query.Subjects.GetSubjectById(parentId)));
            }
            catch (Exception ex)
            {
                return Error(ex.Message);
            }
        }
        
        public string LoadSubjectsUI(int parentId = 0, bool getHierarchy = false, bool isFirst = false)
        {
            return JsonResponse(Subjects.RenderList(Query.Subjects.GetSubjectById(parentId)));
        }
    }
}
