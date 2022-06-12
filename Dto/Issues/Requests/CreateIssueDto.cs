namespace _0sechill.Dto.Issues.Requests
{
    public class CreateIssueDto
    {
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public bool privacy { get; set; }
    }
}
