using System.Collections.Generic;

namespace Query.Blacklists
{
    public static class Domains
    {
        public static List<string> GetList()
        {
            return Sql.Populate<string>("Blacklist_Domains_GetList");
        }

        public static void Add(string domain)
        {
            Sql.ExecuteNonQuery("Blacklist_Domain_Add", new { domain });
        }

        public static void Remove(string domain)
        {
            Sql.ExecuteNonQuery("Blacklist_Domain_Remove", new { domain });
        }

        public static bool Check(string domain)
        {
            return Sql.ExecuteScalar<int>("Blacklist_Domain_Check", new { domain }) > 0;
        }

        public static List<Models.Blacklist> CheckAll(string[] domains)
        {
            return Sql.Populate<Models.Blacklist>("Blacklist_Domains_CheckAll", new { domains = string.Join(",", domains) });
        }

        public static class Wildcards
        {
            public static List<string> GetList()
            {
                return Sql.Populate<string>("Blacklist_Wildcards_GetList");
            }

            public static void Add(string domain)
            {
                Sql.ExecuteNonQuery("Blacklist_Wildcard_Add", new { domain });
            }

            public static void Remove(string domain)
            {
                Sql.ExecuteNonQuery("Blacklist_Wildcard_Remove", new { domain });
            }
        }
    }
}
