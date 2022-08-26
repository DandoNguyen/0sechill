using Microsoft.AspNetCore.SignalR;

namespace _0sechill.Hubs
{
    public class INameUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext hubConnection)
        {
            return hubConnection.User?.Identity?.Name;
        }
    }
}
