using _0sechill.Models.Infrastructure;

namespace _0sechill.Models
{
    public class BookingTask
    {
        public Guid ID { get; set; }
        public DateOnly DateOfBooking { get; set; }
        public TimeOnly TimeLevelOfBooking { get; set; }
        public bool isAvailable { get; set; }

        //FK
        public virtual string userID { get; set; }
        public virtual ApplicationUser User { get; set; }

        public virtual PublicFacility PublicFacility { get; set; }

        public BookingTask()
        {
            ID = Guid.NewGuid();
        }
    }
}
