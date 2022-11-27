using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.FE003.Request
{
    public class CreateIssueDto
    {
        [Required]
        public string title { get; set; }
        [Required]
        public string content { get; set; }
        [Required]
        public bool isPrivate { get; set; }
        public List<IFormFile> listFiles { get; set; }
        public List<string> listCateID { get; set; }

        public CreateIssueDto()
        {
            listCateID = new List<string>();
            listFiles = new List<IFormFile>();
        }

    }
}
