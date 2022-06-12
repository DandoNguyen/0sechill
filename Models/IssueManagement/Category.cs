namespace _0sechill.Models.IssueManagement
{
    public class Category
    {
        public Guid ID { get; set; }
        public string cateName { get; set; }

        //Collection
        public ICollection<Issues> Issues { get; set; }
    }
}
