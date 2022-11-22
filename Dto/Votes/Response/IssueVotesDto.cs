namespace _0sechill.Dto.Votes.Response
{
    public class IssueVotesDto
    {
        public int voteUpCount { get; set; }
        public int voteDownCount { get; set; }
        public List<string> listUserUp { get; set; }
        public List<string> listUserDown { get; set; }
    }
}
