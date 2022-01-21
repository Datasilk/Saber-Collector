using System.Collections.Generic;

namespace Query.Whitelists
{
    public static class Domains
    {
        public static List<string> GetList()
        {
            return Sql.Populate<string>("Whitelist_Domains_GetList");
        }

        public static void Add(string domain)
        {
            Sql.ExecuteNonQuery("Whitelist_Domain_Add", new { domain });
        }
    }
}
