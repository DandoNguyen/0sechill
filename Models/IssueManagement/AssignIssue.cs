using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public bool isResolved { get; set; }
        public ICollection<FilePath> files { get; set; }
        public bool isConfirmed { get; set; }
        public string staffFeedback { get; set; }
    }
}
