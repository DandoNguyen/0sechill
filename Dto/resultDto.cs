namespace _0sechill.Dto
{
    public class resultDto
    {
        public bool isSuccess { get; set; }
        public string message { get; set; }
        public string error { get; set; }

        public resultDto()
        {
            isSuccess = false;
        }
    }
}
