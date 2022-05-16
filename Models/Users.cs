using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class Users
    {
        [Key]
        public Guid userId { get; set; }
        public string userCode { get; set; } = string.Empty;
        public string firstName { get; set; } = string.Empty;
        public string lastName { get; set; } = string.Empty;
        public string citizenId { get; set; } = string.Empty;
        public DateOnly DOB { get; set; }
        public int age { get; set; }
        public string email { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
    }
}
