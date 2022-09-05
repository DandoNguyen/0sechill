using _0sechill.Hubs.Model;

namespace _0sechill.Hubs.Interfaces
{
    public interface IChatHub
    {
        Task SendNotificationToUser(string userId, Notifications notifications);
    }
}
