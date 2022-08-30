namespace _0sechill.Hubs
{
    public interface IHubClient 
    {
        Task Chat(string user, string message);
        Task Notify(string notificationId, string title, string content);
    }
}
