namespace _0sechill.Dto.Issues.Response
{
    public class IssueDto
    {
        public Guid ID { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }

        //TimeStamps
        public DateOnly createdDate { get; set; }
        public DateOnly lastModifiedDate { get; set; }
        public bool privacy { get; set; }
    }
}
