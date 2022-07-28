namespace _0sechill.Dto.Issues.Response
{
    public class assignIssueDto
    {
        public string ID { get; set; }
        public string staffId { get; set; }
        public string issueId { get; set; }
        public bool isResolved { get; set; }
        public List<string> files { get; set; }
        public bool isConfirmed { get; set; }
        public string staffFeedback { get; set; }
    }
}
