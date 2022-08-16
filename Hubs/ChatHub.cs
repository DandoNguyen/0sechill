using Microsoft.AspNetCore.SignalR;

namespace _0sechill.Hubs
{
    public class ChatHub : Hub, IChatHub
    {
        public override async Task OnConnectedAsync()
        {
            await SendMessageAsync("", "User Connected!");
            await base.OnConnectedAsync();
        }



        public async Task SendMessageAsync(string message, string userName)
        {
            await Clients.All.SendAsync("Received Message", userName, message);
        }
    }
}
