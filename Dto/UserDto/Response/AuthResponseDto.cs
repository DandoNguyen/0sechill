namespace _0sechill.Dto.UserDto.Response
{
    public class AuthResponseDto
    {
        public AuthResponseDto()
        {
            success = false;
            message = new List<string>();
            token = "null";
        }
        public bool success { get; set; }
        public List<string> message { get; set; }
        public string token { get; set; }
    }
}
