using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.IssueManagement
{
    public class Comments
    {
        [Key]
        public Guid ID { get; set; }
        public string content { get; set; }
        public bool isPrivate { get; set; }
        public bool isChild { get; set; }
        public Guid parentId { get; set; }

        //FK
        public string authorId { get; set; }
        public ApplicationUser authors { get; set; }
        public Guid issueId { get; set; }
        public Issues issues { get; set; }
    }
}
