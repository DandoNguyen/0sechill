using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Role.Request
{
    public class RemoveRoleDto
    {
        [Required]
        public string userID { get; set; }
        [Required]
        public List<string> listRoles { get; set; }
    }
}
