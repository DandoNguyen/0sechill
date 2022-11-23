namespace _0sechill.Dto.FE001.Response
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string fullname { get; set; }
        public int age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public List<string> roleName { get; set; }

        public UserDto()
        {
            roleName = new List<string>();
        }
    }
}
