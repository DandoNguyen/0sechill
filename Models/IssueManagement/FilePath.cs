namespace _0sechill.Models.IssueManagement
{
    public class FilePath
    {
        public Guid ID { get; set; }
        public string filePath { get; set; }
        public Guid issueId { get; set; } //issue id as owner, user id as owner
        public Issues issues { get; set; }
        public string userId { get; set; }
        public ApplicationUser users { get; set; }
        public Guid assignIssueId { get; set; }
        public AssignIssue assignIssue { get; set; }
    }
}
