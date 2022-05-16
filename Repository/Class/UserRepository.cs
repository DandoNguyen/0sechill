using _0sechill.Data;
using _0sechill.Models;
using _0sechill.Repository.Interface;

namespace _0sechill.Repository.Class
{
    public class UserRepository : Repository<ApplicationUser>, IUserRepository
    {
        public UserRepository(ApiDbContext context) : base(context)
        {
        }
    }
}
