namespace _0sechill.Models.IssueManagement
{
    public class Vote
    {
        public Guid ID { get; set; }
        public bool IsVoteUp { get; set; }
        public Issues issues { get; set; }
        public ApplicationUser User { get; set; }
    }
}
