using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Saber.Core.Extensions.Strings;

namespace Saber.Vendors.Collector
{
    public static class Subjects
    {
        public static int[] Add(string[] subjects, string[] hierarchy)
        {
            var parentId = 0;
            var breadcrumb = "";
            if (hierarchy.Length > 0)
            {
                var parentHier = hierarchy.ToList();
                var parentTitle = hierarchy[hierarchy.Length - 1];
                parentHier.RemoveAt(parentHier.Count - 1);
                var parentBreadcrumb = string.Join(">", parentHier);
                breadcrumb = string.Join(">", hierarchy);
                var subject = Query.Subjects.GetSubjectByTitle(parentTitle, parentBreadcrumb);
                parentId = subject.subjectId;
            }

            var ids = new List<int>();
            foreach (string subject in subjects)
            {
                ids.Add(Query.Subjects.CreateSubject(parentId, 0, 0, subject, breadcrumb));
            }
            return ids.ToArray();
        }

        public static void Move(int subjectId, int newParentId)
        {
            Query.Subjects.Move(subjectId, newParentId);
        }

        #region "Render"
        public static Datasilk.Core.Web.Response RenderList(Query.Models.Subject parent = null)
        {
            var inject = new Datasilk.Core.Web.Response() { };

            var html = new StringBuilder();
            var item = new View("/Vendors/Collector/HtmlComponents/Subjects/list-item.html");
            var subjects = Query.Subjects.GetList("", parent == null ? 0 : parent.subjectId);

            //set up subject sub-items
            if(subjects.Count > 0)
            {
                subjects.ForEach((Query.Models.Subject subject) =>
                {
                    var breadcrumbs = subject.breadcrumb;
                    if (breadcrumbs == "") { breadcrumbs = subject.title; }
                    item["subjectId"] = subject.subjectId.ToString();
                    item["parentId"] = subject.parentId.ToString();
                    item["breadcrumbs"] = subject.breadcrumb.Replace(">", "&gt;") + (subject.breadcrumb != "" ? "&gt;" : "") + subject.title;
                    item["title"] = subject.title.Capitalize();
                    html.Append(item.Render() + "\n");
                });
            }
            else
            {
                html.Append(Cache.LoadFile("/Vendors/Collector/HtmlComponents/Subjects/no-subjects.html"));
            }
            inject.html = Components.Accordion.Render("Subjects", "", html.ToString());
            return inject;
        }
        #endregion

        #region "Dropdown"
        /// <summary>
        /// Generate a list of subjects using HTML option elements
        /// </summary>
        /// <param name="subjectId"></param>
        /// <param name="parent"></param>
        /// <returns>A list of HTML option elements with a value attribute</returns>
        public static string NavigateDropdown(int subjectId, bool parent)
        {
            List<Query.Models.Subject> subjects = null;
            var html = new StringBuilder();
            var root = false;
            if (subjectId == 0)
            {
                //root subjects only
                subjects = Query.Subjects.GetList("", 0);
                root = true;
            }
            else if (parent == true)
            {
                //get subjects for parent
                var subject = Query.Subjects.GetSubjectById(subjectId);
                if (subject.parentId.HasValue)
                {
                    //get parent subjects
                    subjects = Query.Subjects.GetByParentId(subject.parentId.Value);
                }
                else
                {
                    //no parent ID, get root subjects
                    subjects = Query.Subjects.GetList("", 0);
                    root = true;
                }
            }
            else
            {
                //get subjects
                subjects = Query.Subjects.GetByParentId(subjectId);
            }
            html.Append("<option value=\"\" selected>Select a subject...</option>");
            if (root == false)
            {
                var subject = Query.Subjects.GetSubjectById(subjectId);
                html.Append("<option value=\"" + subject.parentId + "\"><< Parent Subject</option>");
                html.Append("<option value=\"0\">All Subjects</option>");
            }
            foreach (var subject in subjects)
            {
                html.Append("<option value=\"" + subject.subjectId + "\"/>" + subject.title + "</option>");
            }
            return html.ToString();
        }
        #endregion
    }
}
