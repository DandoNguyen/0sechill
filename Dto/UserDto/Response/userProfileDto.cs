namespace _0sechill.Dto.UserDto.Response
{
    public class userProfileDto
    {
        public string userCode { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string citizenId { get; set; }
        public DateOnly DOB { get; set; }
        public int age { get; set; }
        public string role { get; set; }
    }
}
