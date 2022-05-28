using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class Words
    {
        public enum GrammarType
        {
            noun = 0,
            properNoun = 1,
            verb = 2,
            adverb = 3,
            adjective = 4

        }

        public static void Add(string word, int subjectId, GrammarType grammarType = GrammarType.noun, int score = 1)
        {
            Sql.ExecuteNonQuery("Word_Add", new { word, subjectId, grammartype = (int)grammarType, score });
        }

        public static void BulkAdd(string[] words, int subjectId)
        {
            Sql.ExecuteNonQuery("Words_BulkAdd", new { words = string.Join(",", words), subjectId });
        }

        public static List<Models.Word> GetList(string[] words)
        {
            return Sql.Populate<Models.Word>("Words_GetList", new { words = string.Join(",", words) });
        }
    }
}
