using System.ComponentModel;

namespace _0sechill.Dto.FE006.Response
{
    public class IssueStaffDto
    {
        public string AssignIssueID { get; set; }
        public string staffId { get; set; }
        public bool isResolved { get; set; }
        public bool isConfirmedByAdmin { get; set; }
        public bool isConfirmed { get; set; }
        public string staffFeedback { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public string status { get; set; }
        public string feedback { get; set; }
        public int priorityLevel { get; set; }
        public List<string> listFileFromStaff { get; set; }
    }
}
