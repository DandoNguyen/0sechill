using _0sechill.Data.Class;
using _0sechill.Data.Interface;

namespace _0sechill.Data
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private readonly ApiDbContext context;
        private IIssueRepository _issue;

        public RepositoryWrapper(ApiDbContext context)
        {
            this.context = context;
        }

        public IIssueRepository Issue {
            get
            {
                if (_issue == null)
                {
                    _issue = new IssueRepository(context);
                }
                return _issue;
            }
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
