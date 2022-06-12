namespace _0sechill.Models.IssueManagement
{
    public class File
    {
        public Guid ID { get; set; }
        public string filePath { get; set; }

        //FK
        public Guid issueId { get; set; }
        public Issues issue { get; set; }
    }
}
