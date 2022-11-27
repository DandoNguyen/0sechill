namespace _0sechill.Dto.FE003.Response
{
    public class IssueDto
    {
        public string ID { get; set; } = String.Empty;
        public string title { get; set; } = String.Empty;
        public string content { get; set; } = String.Empty;
        public string status { get; set; } = String.Empty;
        public int priorityLevel { get; set; } = 0;
        public string feedback { get; set; } = String.Empty;
        public DateTime createdDate { get; set; } = DateTime.MinValue;
        public DateTime lastModifiedDate { get; set; } = DateTime.MinValue;
        public bool isPrivate { get; set; } = false;
        public string authorName { get; set; } = String.Empty;
        public List<string> listCategory { get; set; } = new List<string>();
        public List<string> files { get; set; } = new List<string>();
    }
}
