using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.UserDto.Request
{
    public class UpdateUserDto
    {
        [Required]
        public string userId { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string citizenId { get; set; }
        public int age { get; set; }
        public DateOnly DOB { get; set; }
    }
}
