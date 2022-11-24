namespace _0sechill.Models.IssueManagement
{
    public class FilePath
    {
        public Guid ID { get; set; }
        public string filePath { get; set; }

        //issue id as owner, user id as owner
        public string ownerID { get; set; }

        public FilePath()
        {
            ID = Guid.NewGuid();
        }
    }
}
