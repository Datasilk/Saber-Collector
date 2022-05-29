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
    }
}
