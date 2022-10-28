namespace _0sechill.Models.IssueManagement
{
    public class FilePath
    {
        public Guid ID { get; set; }
        public string filePath { get; set; }

        //issue id as owner, user id as owner
        public virtual Issues issues { get; set; }
        public virtual ApplicationUser users { get; set; }
        public virtual AssignIssue assignIssue { get; set; }

        public FilePath()
        {
            ID = Guid.NewGuid();
        }
    }
}
