using _0sechill.Repository.Interface;

namespace _0sechill.Repository
{
    public interface IRepoWrapper
    {
        IUserRepository User { get; }
        Task Save();
    }
}
