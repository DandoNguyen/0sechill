using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Comments.Request
{
    public class CreateCommentDto
    {
        public string issueId { get; set; }
        public string content { get; set; }
        [Required]
        public bool isChild { get; set; }
        public string parentId { get; set; }
        [Required]
        public bool isPrivate { get; set; }
    }
}
