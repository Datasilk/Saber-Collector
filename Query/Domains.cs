using System;
using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class Domains
    {
        #region "Add"
        public static int Add(string domain, string title = "", int parentId = 0, int type = 0)
        {
            return Sql.ExecuteScalar<int>("Domain_Add", new { domain, title, parentId, type });
        }
        #endregion

        #region "Get"

        public static List<Models.Domain>GetList(int[] subjectIds = null, Models.DomainFilterType type = Models.DomainFilterType.All, Models.DomainType domainType = Models.DomainType.unused, Models.DomainSort sort = Models.DomainSort.Alphabetical, string search = "", int start = 1, int length = 50, int parentId = -1)
        {
            return Sql.Populate<Models.Domain>("Domains_GetList", new { subjectIds = string.Join(",", subjectIds), search, type, domainType, sort, start, length, parentId });
        }

        public static int GetCount(int[] subjectIds = null, Models.DomainFilterType type = Models.DomainFilterType.All, Models.DomainType domainType = Models.DomainType.unused, Models.DomainSort sort = Models.DomainSort.Alphabetical, string search = "", int parentId = -1)
        {
            return Sql.ExecuteScalar<int>("Domains_GetCount", new { subjectIds = string.Join(",", subjectIds), search, type, domainType, sort, parentId });
        }

        public static Models.Domain GetInfo(string domain)
        {
            return Sql.Populate<Models.Domain>("Domain_GetInfo", new { domain }).FirstOrDefault();
        }

        public static Models.Domain GetById(int domainId)
        {
            return Sql.Populate<Models.Domain>("Domain_GetById", new { domainId }).FirstOrDefault();
        }

        #endregion

        #region "Links"
        public static List<Models.Domain>GetLinks(int domainId)
        {
            return Sql.Populate<Models.Domain>("DomainLinks_GetList", new { domainId });
        }
        #endregion

        #region "Analyzer Rules"
        public static class AnalyzerRules
        {
            public static int Add(int domainId, string selector, bool rule)
            {
                return Sql.ExecuteScalar<int>("Domain_AnalyzerRule_Add", new { domainId, selector, rule });
            }

            public static List<Models.AnalyzerRule> GetList(int domainId)
            {
                return Sql.Populate<Models.AnalyzerRule>("Domain_AnalyzerRules_GetList", new { domainId });
            }
            public static void Remove(int ruleId)
            {
                Sql.ExecuteNonQuery("Domain_AnalyzerRule_Remove", new { ruleId });
            }
        }
        #endregion

        #region "Download Rules"
        public static class DownloadRules
        {
            public static int Add(int domainId, bool rule, string url, string title, string summary)
            {
                return Sql.ExecuteScalar<int>("Domain_DownloadRule_Add", new { domainId, rule, url, title, summary });
            }

            public static List<Models.DownloadRule> GetList(int domainId)
            {
                return Sql.Populate<Models.DownloadRule>("Domain_DownloadRules_GetList", new { domainId });
            }

            public static List<Models.DownloadRule> GetForDomains(string[] domains)
            {
                return Sql.Populate<Models.DownloadRule>("Domain_DownloadRules_GetForDomains", new { domains = string.Join(",", domains) });
            }
            public static void Remove(int ruleId)
            {
                Sql.ExecuteNonQuery("Domain_DownloadRule_Remove", new { ruleId });
            }
        }
        #endregion

        #region "Clean"

        public static Models.CleanDownload GetDownloadsToClean(int domainId, bool topten = false)
        {
            using (var conn = new Connection("Domain_GetDownloadsToClean", new { domainId, topten }))
            {
                var clean = new Models.CleanDownload();
                var results = conn.PopulateMultiple();
                clean.totalArticles = results.Read<int>().FirstOrDefault();
                clean.articles = results.Read<Models.Article>().ToList();
                clean.totalDownloads = results.Read<int>().FirstOrDefault();
                return clean;
            }
        }

        public static void CleanDownloads(int domainId)
        {
            Sql.ExecuteNonQuery("Domain_CleanDownloads", new { domainId});
        }

        #endregion

        #region "Delete"

        public static void DeleteAllArticles(int domainId)
        {
            Sql.ExecuteNonQuery("Domain_DeleteAllArticles", new { domainId });
        }

        public static void Delete(int domainId)
        {
            Sql.ExecuteNonQuery("Domain_Delete", new { domainId });
        }

        #endregion

        #region "Update"

        public static void RequireSubscription(int domainId, bool required)
        {
            Sql.ExecuteNonQuery("Domain_RequireSubscription", new { domainId, required });
        }

        public static void HasFreeContent(int domainId, bool free)
        {
            Sql.ExecuteNonQuery("Domain_HasFreeContent", new { domainId, free });
        }

        public static string FindDomainTitle(int domainId)
        {
            return Sql.ExecuteScalar<string>("Domain_FindTitle", new { domainId });
        }

        public static string FindDescription(int domainId)
        {
            return Sql.ExecuteScalar<string>("Domain_FindDescription", new { domainId });
        }

        public static void UpdateDescription(int domainId, string title, string description, string lang)
        {
            Sql.ExecuteNonQuery("Domain_UpdateDescription", new { domainId, title, description, lang });
        }

        public static void UpdateDomainType(int domainId, Models.DomainType type)
        {
            Sql.ExecuteNonQuery("Domain_UpdateType", new { domainId, type = (int)type });
        }

        public static void UpdateHttpsWww(int domainId, bool https, bool www)
        {
            Sql.ExecuteNonQuery("Domain_UpdateHttpsWww", new { domainId, https, www });
        }

        public static void IsEmpty(int domainId, bool empty)
        {
            Sql.ExecuteNonQuery("Domain_IsEmpty", new { domainId, empty });
        }

        public static void IsDeleted(int domainId, bool delete)
        {
            Sql.ExecuteNonQuery("Domain_IsDeleted", new { domainId, delete });
        }

        #endregion

        #region "Collections"
        public static class Collections
        {
            public static int Add(int colgroupId, string name, string search = "", int subjectId = 0, Models.DomainFilterType filtertype = Models.DomainFilterType.All, Models.DomainType type = Models.DomainType.unused, Models.DomainSort sort = Models.DomainSort.Alphabetical)
            {
                return Sql.ExecuteScalar<int>("Domain_Collection_Add", new { colgroupId, name, search, subjectId, filtertype, type, sort });
            }

            public static int Add(Models.DomainCollection collection)
            {
                return Add(collection.colgroupId, collection.name, collection.search, collection.subjectId, collection.filtertype, collection.type, collection.sort);
            }

            public static Models.DomainCollectionsAndGroups GetList()
            {
                using(var conn = new Connection("Domain_Collections_GetList"))
                {
                    var reader = conn.PopulateMultiple();

                    var collections = reader.Read<Models.DomainCollection>().ToList();
                    var groups = reader.Read<Models.CollectionGroup>().ToList();
                    return new Models.DomainCollectionsAndGroups()
                    {
                        Collections = collections,
                        Groups = groups
                    };
                }
            }

            public static int Remove(int colId)
            {
                return Sql.ExecuteScalar<int>("Domain_Collection_Remove", new { colId });
            }
        }
        #endregion

        #region "Collection Groups"
        public static class CollectionGroups
        {
            public static int Add(string name)
            {
                return Sql.ExecuteScalar<int>("Domain_CollectionGroup_Add", new { name });
            }

            public static int Remove(int colgroupId)
            {
                return Sql.ExecuteScalar<int>("Domain_CollectionGroup_Remove", new { colgroupId });
            }

            public static List<Models.CollectionGroup> GetList()
            {
                return Sql.Populate<Models.CollectionGroup>("Domain_CollectionGroups_GetList");
            }
        }
        #endregion
    }
}
