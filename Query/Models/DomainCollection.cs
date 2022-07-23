using System;

namespace Query.Models
{
    public class DomainCollection
    {
        public int colId { get; set; }
        public int colgroupId { get; set; }
        public string name { get; set; }
        public string search { get; set; }
        public int subjectId { get; set; }
        public DomainType type { get; set; }
        public DomainSort sort { get; set; }
        public DateTime datecreated { get; set; }
        public string groupName { get; set; }
    }
}
