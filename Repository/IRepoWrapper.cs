using _0sechill.Repository.Interface;

namespace _0sechill.Repository
{
    public interface IRepoWrapper
    {
        IUserRepository ApplicationUser { get; }
        Task Save();
    }
}
