using _0sechill.Hubs.Model;

namespace _0sechill.Hubs
{
    public interface IChatHub 
    {
        Task SendNotificationToUser(string userId, Notifications notifications);
    }
}
