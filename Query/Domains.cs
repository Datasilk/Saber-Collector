using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class Domains
    {

        public static List<Models.Domain>GetList(int[] subjectIds = null, int type = 0, int sort = 0, string search = "", int start = 1, int length = 50)
        {
            return Sql.Populate<Models.Domain>("Domains_GetList", new { subjectIds = string.Join(",", subjectIds), search, type, sort, start, length });
        }

        public static Models.Domain GetInfo(string domain)
        {
            return Sql.Populate<Models.Domain>("Domain_GetInfo", new { domain }).FirstOrDefault();
        }

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
    }
}
