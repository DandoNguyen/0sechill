using _0sechill.Data.Interface;
using _0sechill.Models.IssueManagement;

namespace _0sechill.Data.Class
{
    public class IssueRepository : RepositoryBase<Issues>, IIssueRepository
    {

        public IssueRepository(ApiDbContext context) : base(context)
        {
        }


    }
}
