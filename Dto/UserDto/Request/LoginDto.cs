using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.UserDto.Request
{
    public class LoginDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
