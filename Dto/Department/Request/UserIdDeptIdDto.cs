using System.ComponentModel.DataAnnotations;

namespace _0sechill.Dto.Department.Request
{
    public class UserIdDeptIdDto
    {
        [Required]
        public string userId { get; set; }
        [Required]
        public string deptId { get; set; }
    }
}
