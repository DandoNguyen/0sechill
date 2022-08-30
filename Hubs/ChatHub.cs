using _0sechill.Data;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Hubs
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatHub : Hub<IHubClient>
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

        /// <summary>
        /// this is the method connect the user with the current connection id
        /// </summary>
        /// <returns></returns>
        public override async Task<Task> OnConnectedAsync()
        {
            var userId = Context.User.Identity.Name;
            string currentConnectionId = Context.ConnectionId;

            var user = await userManager.FindByIdAsync(userId);
            user.currentHubConnectionId = currentConnectionId;
            await userManager.UpdateAsync(user);
            await context.SaveChangesAsync();
            await LoadUnseenNotificationAsync(userId);
            return base.OnConnectedAsync();
        }

        /// <summary>
        /// this is the method that disconnected the user and remove the connectionId
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public override async Task<Task> OnDisconnectedAsync(Exception exception)
        {
            var user = await userManager.FindByIdAsync(Context.User.Identity.Name);
            user.currentHubConnectionId = null;
            await userManager.UpdateAsync(user);

            await Clients.User(user.Id).Chat("System", "Hub Disconnected!");
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// this is the method that send notification to a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notifications"></param>
        /// <returns></returns>
        public async Task SendNotificationToUser(string userId, Notifications notifications)
        {
            var existNotif = await context.notifications.Where(x => x.ID.Equals(notifications.ID)).FirstOrDefaultAsync();
            if (existNotif is null)
            {
                await context.notifications.AddAsync(notifications);
                await context.SaveChangesAsync();
            }

            var user = await userManager.FindByIdAsync(userId);
            if (!string.IsNullOrEmpty(user.currentHubConnectionId))
            {
                await Clients.Client(user.currentHubConnectionId).Notify(notifications.title, notifications.content);
            }
        }

        /// <summary>
        /// this is the method to load any unread notification of logged in user on hub connection events
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        private async Task LoadUnseenNotificationAsync(string userId)
        {
            var listNotif = await context.notifications
                .Where(x => x.receiverId.Equals(userId))
                .Where(x => x.isSeen.Equals(false))
                .ToListAsync();
            if (!listNotif.Count.Equals(0))
            {
                foreach (var notif in listNotif)
                {
                    await SendNotificationToUser(userId, notif);
                }
            }
        }

        /// <summary>
        /// Represent the method sending message to a group of user
        /// </summary>
        /// <param name="Authorization"></param>
        /// <param name="message"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HubMethodName("SendMessageToGroup")]
        public async Task SendMessageToGroupAsync(string Authorization, string message, string roomId)
        {
            var user = await tokenService.DecodeTokenAsync(Authorization);
            var existRoom = await context.chatRooms.FindAsync(roomId);

            await SendMessageAsync(user, message, existRoom);
        }

        /// <summary>
        /// represent the method connecting to group chat
        /// </summary>
        /// <param name="Authorization"></param>
        /// <param name="roomId"></param>
        /// <returns></returns>
        [HubMethodName("ConnectToGroupChat")]
        public async Task ConnectToGroupChat(string Authorization, string roomId)
        {
            var user = await tokenService.DecodeTokenAsync(Authorization);
            var existRoom = await context.chatRooms.FindAsync(roomId);
            await Groups.AddToGroupAsync(Context.ConnectionId, existRoom.roomName);
        }

        /// <summary>
        /// Represent the method sending message to a single user
        /// </summary>
        /// <param name="receiverId"></param>
        /// <param name="message"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HubMethodName("SendMessageToUser")]
        public async Task SendMessageToUser([Required] string receiverId, [Required] string message)
        {
            var senderId = Context.User.Identity.Name;
            var sender = await userManager.FindByIdAsync(senderId);
            if (sender is null)
            {
                await Clients.All.Chat("error", "sender is null");
                return;
            }

            //Try find exist Room
            var existRoom = await FindExistRoomAsync(sender.Id, receiverId);
            if (existRoom is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, existRoom.ID.ToString());
                await SendMessageAsync(sender, message, existRoom);
            }
            //Create new Room when exist Room is null
            else
            {
                var newRoom = new Room();
                newRoom.isGroupChat = false;
                await context.chatRooms.AddAsync(newRoom);
                await context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, newRoom.ID.ToString());
                await SendMessageAsync(sender, message, newRoom);
            }
        }
        
        /// <summary>
        /// represent the method recording message everytime it is called
        /// </summary>
        /// <param name="author"></param>
        /// <param name="message"></param>
        /// <param name="room"></param>
        /// <returns></returns>
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

        /// <summary>
        /// represent the method finding the exist room between two people
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        private async Task<Room> FindExistRoomAsync(string senderId, string receiverId)
        {
            var userFirst = await userManager.FindByIdAsync(senderId);
            var listRoomfromFirst = await context.chatRooms.Where(x => x.users.Contains(userFirst)).ToListAsync();

            var userSecond = await userManager.FindByIdAsync(receiverId);
            var listRoomFromSecond = await context.chatRooms.Where(x => x.users.Contains(userSecond)).ToListAsync();

            for (int i = 0; i < listRoomFromSecond.Count; i++)
            {
                foreach (var room in listRoomfromFirst)
                {
                    if (listRoomFromSecond[i].ID.Equals(room.ID) && !room.isGroupChat)
                    {
                        return room;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// represent the method sending message
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="message"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        [HubMethodName("SendMessage")]
        public async Task SendMessageAsync(ApplicationUser sender, string message, Room room)
        {

            await RecordMessagesAsync(sender, message, room);
            try
            {
                if (!string.IsNullOrEmpty(room.roomName))
                {
                    await Clients.Group(room.roomName).Chat(sender.UserName, message);

                }
                else
                {
                    await Clients.All.Chat(sender.UserName, message);
                }
            }
            catch (Exception ex)
            {
                await Clients.All.Chat("System Exception", ex.Message);
            }
        }
    }
}
