using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.IssueManagement
{
    public class AssignIssue
    {
        [Key]
        public Guid ID { get; set; }
        public string staffId { get; set; }
        public ApplicationUser staff { get; set; }
        public Guid issueId { get; set; }
        public Issues Issue { get; set; }
    }
}
