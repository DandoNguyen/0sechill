namespace _0sechill.Dto.FE003.Response
{
    public class IssueDto
    {
        public string ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
        public DateTime createdDate { get; set; }
        public DateTime lastModifiedDate { get; set; }
        public bool isPrivate { get; set; }
        public string authorName { get; set; }
        public List<string> listCategory { get; set; }
        public List<string> files { get; set; }
    }
}
