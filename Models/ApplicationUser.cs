using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Key]
        public Guid userId { get; set; }
        public string userCode { get; set; } 
        public string firstName { get; set; }
        public string lastName { get; set; } 
        public string citizenId { get; set; }
        public DateOnly DOB { get; set; }
        public int age { get; set; }
        public string role { get; set; } 
    }
}
