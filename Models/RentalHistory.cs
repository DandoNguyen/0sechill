using System.ComponentModel.DataAnnotations;

namespace _0sechill.Models
{
    public class RentalHistory
    {
        [Key]
        public Guid rentalHistoryId { get; set; }
        [Required]
        public DateOnly startDate { get; set; }
        [Required]
        public DateOnly endDate { get; set; }
        [Required]
        public DateOnly lastSignedDate { get; set; } = DateOnly.FromDateTime(new DateTime());
        [Required]
        public string status { get; set; }

        //Timestamps
        [Required]
        public DateTimeOffset createdDate { get; set; } = new DateTimeOffset();
        [Required]
        public DateTimeOffset modifiedDate { get; set; } = new DateTimeOffset();

        //FK
        public ApplicationUser applicationUser { get; set; }
        public Guid userId { get; set; }

        public Apartment apartment { get; set; }
        public Guid apartmentId { get; set; }
    }
}
