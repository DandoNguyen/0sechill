namespace _0sechill.Hubs
{
    public interface IHubClient 
    {
        Task Chat(string user, string message, string roomId, string roomName);
        Task Notify(string notificationId, string title, string content);
    }
}
