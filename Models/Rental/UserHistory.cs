using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class UserHistory // hop dong
    {
        [Key]
        public Guid userHistoryId { get; set; }
        [Required]
        public DateTime startDate { get; set; } = DateTime.MinValue;
        [Required]
        public DateTime endDate { get; set; } = DateTime.MinValue;
        [Required]
        public DateTime lastSignedDate { get; set; } = DateTime.MinValue;
        
        public string status { get; set; }

        //Timestamps
        [Required]
        public DateTimeOffset createdDate { get; set; } = new DateTimeOffset();
        [Required]
        public DateTimeOffset modifiedDate { get; set; } = new DateTimeOffset();


        //FK
        public ApplicationUser applicationUser { get; set; }
        public Guid userId { get; set; }

        public ICollection<Apartment> apartment { get; set; }
        public Guid apartmentId { get; set; }

        public UserHistory()
        {
            userHistoryId = Guid.NewGuid();
        }
    }
}
