using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Issues.Requests
{
    public class AssignIssueStaffDto
    {
        [Required]
        public string issueId { get; set; }
        [Required]
        public string staffId { get; set; }
    }
}
