using _0sechill.Data;
using _0sechill.Repository.Class;
using _0sechill.Repository.Interface;

namespace _0sechill.Repository
{
    public class RepoWrapper : IRepoWrapper
    {
        private ApiDbContext context;
        private IUserRepository _user;

        public RepoWrapper(ApiDbContext context)
        {
            this.context = context;
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(context);
                }
                return _user;
            }
        }

        public async Task Save()
        {
            await context.SaveChangesAsync();
        }
    }
}
