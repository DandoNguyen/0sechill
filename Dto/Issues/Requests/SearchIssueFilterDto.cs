namespace _0sechill.Dto.Issues.Requests
{
    public class SearchIssueParamsDto
    {
        public string title { get; set; }
        public string authorUsername { get; set; }
        public DateOnly dateFrom { get; set; }
        public DateOnly dateTo { get; set; }
    }
}
