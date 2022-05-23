using System.Collections.Generic;
using System.Linq;

namespace Query
{
    public static class Domains
    {

        public static List<Models.Domain>GetList(int[] subjectIds = null, string search = "", int start = 1, int length = 50)
        {
            return Sql.Populate<Models.Domain>("Domains_GetList", new { subjectIds = string.Join(",", subjectIds), search, start, length });
        }
    }
}
