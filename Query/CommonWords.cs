using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class CommonWords
    {
        public static void Add(string[] words)
        {
            Sql.ExecuteNonQuery("CommonWords_Add", new { words = string.Join(",", words) });
        }

        public static List<string> GetList()
        {
            return Sql.Populate<Models.CommonWord>("CommonWords_GetList").Select(a => a.word).ToList();
        }
    }
}
