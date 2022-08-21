using _0sechill.Data;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Hubs
{
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
        public async Task SendMessageToUser([Required] string receiverId, [Required] string message, [Required] string token)
        {
            var sender = await tokenService.DecodeToken(token);

            //Try find exist Room
            var existRoomId = await FindExistRoomAsync(sender.Id, receiverId, isGroupChat: false);
            if (!string.IsNullOrEmpty(existRoomId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, existRoomId);
                await SendMessageAsync(sender.UserName, message, existRoomId);
            }
            //Create new Room when exist Room is null
            else
            {
                var newRoom = new Room();
                newRoom.isGroupChat = false;
                await context.chatRooms.AddAsync(newRoom);
                await context.SaveChangesAsync();
                await RecordMessagesAsync(sender, message, newRoom);

                await Groups.AddToGroupAsync(Context.ConnectionId, newRoom.ID.ToString());
                await SendMessageAsync(sender.UserName, message, newRoom.ID.ToString());
            }
        }
            
        private async Task RecordMessagesAsync(ApplicationUser author, string message, Room room)
        {
            var newMessage = new Message();
            newMessage.message = message;
            newMessage.userId = author.Id;
            newMessage.User = author;
            newMessage.roomId = room.ID;
            newMessage.Room = room;

            await context.chatMessages.AddAsync(newMessage);
            await context.SaveChangesAsync();
        }

        private async Task<string> FindExistRoomAsync(string senderId, string receiverId, bool isGroupChat)
        {
            var listRooms = await context.chatRooms
                .Where(x => x.users.Any(y => y.Id.Equals(senderId)) && x.users.Any(y => y.Id.Equals(receiverId)))
                .ToListAsync();
            foreach (var room in listRooms)
            {
                if (room.isGroupChat.Equals(isGroupChat))
                {
                    return room.ID.ToString();
                }
            }
            return null;
        }

        [HubMethodName("SendMessage")]
        public async Task SendMessageAsync(string sender, string message, string roomName)
        {
            try
            {
                if (!string.IsNullOrEmpty(roomName))
                {
                    await Clients.Group(roomName).SendAsync("", sender, message);

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

        //public override async Task OnConnectedAsync()
        //{
        //    var roomName = Context.GetHttpContext().Request.Query["roomName"].ToString();
        //    if (!string.IsNullOrEmpty(roomName))
        //    {
        //        await Groups.AddToGroupAsync(Context.ConnectionId, roomName);
        //    }
        //    else
        //    {
        //        await SendMessageAsync("test sender", "no room id provided (TEST)", "");
        //    }
        //    await base.OnConnectedAsync();
        //}

        //public string GetConnectionId() => Context.ConnectionId;

        //public override async Task OnDisconnectedAsync(Exception? exception)
        //{
        //    var token = Context.GetHttpContext().Request.Headers.Authorization;
        //    var user = await tokenService.DecodeToken(token);
        //    if (user is not null)
        //    {
        //        var sender = user.UserName;
        //        var message = $"{user.UserName} has left the room";
        //        await SendMessageAsync(sender, message, "");
        //    }
        //    await base.OnDisconnectedAsync(exception);
        //}
    }
}
