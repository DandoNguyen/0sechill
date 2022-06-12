using _0sechill.Models.IssueManagement;
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
        public DateOnly DOB { get; set; }
        public string role { get; set; }

        //Refresh Token
        public string Token { get; set; }
        public DateTime TokenCreatedDate { get; set; }
        public DateTime TokenExpireDate { get; set; }

        //Collection offset
        public ICollection<UserHistory> userHistories { get; set; }
        public ICollection<Issues> issues { get; set; } 
    }
}
