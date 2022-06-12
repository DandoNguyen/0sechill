namespace _0sechill.Dto.Apartment.Response
{
    public class ApartmentDto
    {
        public Guid apartmentId { get; set; }
        public string apartmentName { get; set; }
        public int heartWallArea { get; set; }
        public int clearanceArea { get; set; }
        public int bedroomAmount { get; set; }
    }
}
