using System;
using System.Collections.Generic;
using System.Text;
using Saber.Core;
using Saber.Vendor;

namespace Saber.Vendors.Collector.HtmlComponents.Subjects
{
    public class Subjects : IVendorHtmlComponents
    {
        public List<HtmlComponentModel> Bind()
        {
            return new List<HtmlComponentModel>()
            {
                new HtmlComponentModel()
                {
                    Key = "collector-subjects",
                    Name = "Collector Subjects",
                    Description = "Navigate through a hierarchy of subjects used for categorizing articles",
                    ContentField = true,
                    Icon = "/Vendors/Collector/icon.svg",
                    Parameters = new Dictionary<string, HtmlComponentParameter>(),
                    Render = new Func<View, IRequest, Dictionary<string, string>, Dictionary<string, object>, string, string, List<KeyValuePair<string, string>>>((view, request, args, data, prefix, key) =>
                    {
                        var results = new List<KeyValuePair<string, string>>();

                        //render subjects
                        var component = new View("/Vendors/Collector/HtmlComponents/Subjects/htmlcomponent.html");
                        var subjectId = request.Parameters.ContainsKey("subjectId") ? int.Parse(request.Parameters["subjectId"]) : 0;
                        var subject = subjectId > 0 ? Query.Subjects.GetSubjectById(subjectId) : null;
                        var subjects = Collector.Subjects.RenderList(subject);
                        component["subjects"] = subjects.html;
                        if(subjectId > 0)
                        {
                            //parent subject information
                            component.Show("has-parent");
                            var crumb = subject.breadcrumb.Replace(">", " &gt; ");
                            var indexes = new string[] { };
                            if (subject.parentId == 0) { crumb = subject.title; } else { crumb += " &gt; " + subject.title; }
                            indexes = subject.hierarchy.Split('>');
                            component["parent"] = subject.parentId > 0 ? "subjectId=" + subject.parentId.ToString() : "?";
                            component["breadcrumbs"] = crumb;
                        }
                        

                        //add CSS & JS files
                        request.AddCSS("/editor/vendors/collector/htmlcomponents/Subjects/subjects.css", "collector_subjects_css");
                        request.AddScript("/editor/vendors/collector/htmlcomponents/Subjects/subjects.js", "collector_subjects_js");

                        //append script blocks
                        if(subjects.javascript != ""){request.AddScriptBlock(subjects.javascript); }

                        results.Add(new KeyValuePair<string, string>(prefix + key, component.Render()));
                        return results;
                    })
                }
            };
        }
    }
}
