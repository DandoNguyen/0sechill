using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Issues.Requests
{
    public class IssueReviewResultDto
    {
        [Required]
        public string issueId { get; set; }
        [Required]
        public bool isVerified { get; set; }
        public string feedback { get; set; }
    }
}
