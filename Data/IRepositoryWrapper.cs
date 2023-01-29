using _0sechill.Data.Interface;

namespace _0sechill.Data
{
    public interface IRepositoryWrapper
    {
        //Interfaces
        IIssueRepository Issue { get; }

        //Common Class
        void Save();
    }
}
