using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Issues.Requests
{
    public class StaffComfirmDto
    {
        [Required]
        public string assignIssueId { get; set; }
        [Required]
        public bool isComfirmed { get; set; }
        public string staffFeedback { get; set; }
    }
}
