using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models.Dto.UserDto.Request
{
    public class RegistrationDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string citizenId { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string password { get; set; }
    }
}
