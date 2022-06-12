namespace _0sechill.Dto.UserDto.Response
{
    public class AuthResponseDto
    {
        public AuthResponseDto()
        {
            success = false;
            message = "null";
            token = "null";
        }
        public bool success { get; set; }
        public string message { get; set; }
        public string token { get; set; }
    }
}
