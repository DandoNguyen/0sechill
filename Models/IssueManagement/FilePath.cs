namespace _0sechill.Models.IssueManagement
{
    public class FilePath
    {
        public Guid ID { get; set; }
        public string filePath { get; set; }

        //FK
        public Guid issueId { get; set; }
        public Issues issue { get; set; }
    }
}
