using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Role.Request
{
    public class AssignRoleDto
    {
        [Required]
        public string roleName { get; set; }
        [Required]
        public string userId { get; set; }
    }
}
