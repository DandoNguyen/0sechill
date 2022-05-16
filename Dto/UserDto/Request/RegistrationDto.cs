using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.Dto.UserDto.Request
{
    public class RegistrationDto
    {
        [Required]
        public string email { get; set; }
        [Required]
        public string password { get; set; }
    }
}
