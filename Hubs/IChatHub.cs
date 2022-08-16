namespace _0sechill.Hubs
{
    public interface IChatHub
    {
        public Task OnConnectedAsync();
        public Task OnDisconnectedAsync();
        public Task SendMessageAsync(string message, string userName);
    }
}
