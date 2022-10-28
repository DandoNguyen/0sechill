using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE003.Request
{
    public class CreateIssueDto
    {
        [Required]
        public string content { get; set; }
        [Required]
        public bool isPrivate { get; set; }
        public List<IFormFile> listFiles { get; set; }
        public List<string> listCateID { get; set; }

    }
}
