namespace Model
{
    using System.Collections.Generic;

    public class Issue
    {
        public long   Id   { get; set; }
        public string Name { get; set; }

        public List<string> Tags { get; set; }

        public long? AssigneeId { get; set; }
        public long? ReporterId { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }
}