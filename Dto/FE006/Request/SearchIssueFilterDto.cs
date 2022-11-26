using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE006.Request
{
    public class SearchIssueFilterDto
    {
        public string status { get; set; }
        [Required]
        public int priorityLevel { get; set; }
        [Required]
        public string title { get; set; }
    }
}
