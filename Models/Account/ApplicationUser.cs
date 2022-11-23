using _0sechill.Hubs.Model;
using _0sechill.Models.Account;
using _0sechill.Models.IssueManagement;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string userCode { get; set; } 
        public string firstName { get; set; }
        public string lastName { get; set; }
        public DateTime DOB { get; set; }
        public bool isActive { get; set; }
        public int age { get; set; }
        public bool isMale { get; set; }
        public string IDType { get; set; }
        public string IDNumber { get; set; }
        public string roleID { get; set; }
        public string residentialAddress { get; set; }
        public string phoneCountryCode { get; set; }

        public string role { get; set; }
        public string currentHubConnectionId { get; set; }

        //Refresh Token
        public string Token { get; set; }
        public DateTime TokenCreatedDate { get; set; }
        public DateTime TokenExpireDate { get; set; }

        //Assign Block offset if user is manager
        public Block block { get; set; }

        //Foreign Key offset
        public virtual Department department { get; set; }

        #region Collection offset
        public ICollection<UserHistory> userHistories { get; set; }
        public ICollection<Issues> issues { get; set; }
        public ICollection<Comments> comments { get; set; }
        public ICollection<AssignIssue> assignIssues { get; set; }
        public ICollection<Message> Messages { get; set; }
        public ICollection<Room> chatRooms { get; set; }
        public ICollection<BookingTask> bookingTasks { get; set; }
        public ICollection<Vote> votes { get; set; }

        #endregion

        public ApplicationUser()
        {
            isActive = true;
        }
    }
}
