namespace _0sechill.Models.Infrastructure
{
    public class PublicFacility
    {
        public Guid ID { get; set; }
        public string typeOfPublic { get; set; }
        public string facilityCode { get; set; }

        //FK
        public BookingTask BookingTask { get; set; }
    }
}
