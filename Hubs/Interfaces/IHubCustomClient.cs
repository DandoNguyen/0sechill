namespace _0sechill.Hubs.Interfaces
{
    public interface IHubCustomClient
    {
        Task Chat(string user, string message, string roomId, string roomName);
        Task Notify(string notificationId, string title, string content);
        Task MediaNotify(string from, string Message, string signal);
    }
}
