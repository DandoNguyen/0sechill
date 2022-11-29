namespace _0sechill.Dto.FE004.Response
{
    public class BookingTaskDto
    {
        public string ID { get; set; }
        public DateTime DateOfBooking { get; set; }
        public bool isAvailable { get; set; }
        public string UserName { get; set; }
        public List<string> listFacil { get; set; }
    }
}
