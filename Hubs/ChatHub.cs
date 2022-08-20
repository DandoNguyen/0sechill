using _0sechill.Data;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;

        public ChatHub(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService)
        {
            this.context = context;
            this.userManager = userManager;
            this.tokenService = tokenService;
        }

        [HubMethodName("SendMessageToUser")]
        public async Task SendMessageToUser([Required] string receiverId, [Required] string message)
        {
            var token = Context.GetHttpContext().Request.Headers.Authorization;
            var sender = await tokenService.DecodeToken(token);

            var receiver = await userManager.FindByIdAsync(receiverId);

            string roomName = sender.Id + receiver.Id;
            await Groups.AddToGroupAsync(Context.ConnectionId, Context.UserIdentifier.ToString() + $"{roomName}");
            await SendMessageAsync(sender.UserName, message, roomName);
        }

        public override async Task OnConnectedAsync()
        {
            var roomName = Context.GetHttpContext().Request.Query["roomName"].ToString();
            if (!string.IsNullOrEmpty(roomName))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
            }
            else
            {
                await SendMessageAsync("test sender", "no room id provided (TEST)", "");
            }
            await base.OnConnectedAsync();
        }

        public string GetConnectionId() => Context.ConnectionId;

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            var token = Context.GetHttpContext().Request.Headers.Authorization;
            var user = await tokenService.DecodeToken(token);
            if (user is not null)
            {
                var sender = user.UserName;
                var message = $"{user.UserName} has left the room";
                await SendMessageAsync(sender, message, "");
            }
            await base.OnDisconnectedAsync(exception);
        }

        [HubMethodName("SendMessage")]
        public async Task SendMessageAsync(string sender, string message, string? roomName)
        {
            try
            {
                if (string.IsNullOrEmpty(roomName))
                {
                    await Clients.All.SendAsync("", sender, message);

                }
                else
                {
                    await Clients.All.SendAsync("", sender, message);
                }
            }
            catch (Exception ex)
            {
                await Clients.All.SendAsync("", ex);
            }
        }
    }
}
