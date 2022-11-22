using _0sechill.Data;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Interfaces;
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
    public class ChatHub : Hub<IHubCustomClient>, IChatHub
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
            var userId = Context.UserIdentifier;
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
            var user = await userManager.FindByIdAsync(Context.UserIdentifier);
            user.currentHubConnectionId = null;
            await userManager.UpdateAsync(user);
                        
            return base.OnDisconnectedAsync(exception);
        }

        /// <summary>
        /// this is the method that send notification to a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="notif"></param>
        /// <returns></returns>
        public async Task SendNotificationToUser(string userId, Notifications notif)
        {
            var existNotif = await context.notifications.Where(x => x.ID.Equals(notif.ID)).FirstOrDefaultAsync();
            if (existNotif is null)
            {
                existNotif.receiverId = userId;
                await context.notifications.AddAsync(notif);
            }
            else if (string.IsNullOrEmpty(existNotif.receiverId))
            {
                existNotif.receiverId = userId;
                context.notifications.Update(existNotif);
            }

            var user = await userManager.FindByIdAsync(userId);
            if (!string.IsNullOrEmpty(user.currentHubConnectionId))
            {
                await Clients.Client(user.currentHubConnectionId).Notify(notif.ID.ToString(), notif.title, notif.content);
            }

            await context.SaveChangesAsync();
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
        /// this is the method that check if the user has seen the notification
        /// </summary>
        /// <param name="notifId"></param>
        /// <returns></returns>
        [HubMethodName("NotificationSeen")]
        public async Task NotificationSeenAsync(string notifId)
        {
            var existNotif = await context.notifications.FindAsync(Guid.Parse(notifId));
            if (existNotif is not null)
            {
                existNotif.isSeen = true;
                context.notifications.Update(existNotif);
                await context.SaveChangesAsync();
            }
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
            var senderId = Context.UserIdentifier;

            //Try find exist Room
            var existRoom = await FindExistRoomAsync(senderId, receiverId);
            if (existRoom is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, existRoom.ID.ToString());
                await UpdateMessagesStatusAsync(existRoom.ID.ToString());
                await SendMessageAsync(senderId, message, existRoom);
            }
            //Create new Room when exist Room is null
            else
            {
                var newRoom = new Room();
                newRoom.isGroupChat = false;
                await context.chatRooms.AddAsync(newRoom);
                await context.SaveChangesAsync();

                await Groups.AddToGroupAsync(Context.ConnectionId, newRoom.ID.ToString());
                await UpdateMessagesStatusAsync(newRoom.ID.ToString());
                await SendMessageAsync(senderId, message, newRoom);
            }
        }

        /// <summary>
        /// this is the method that send message to specific room
        /// </summary>
        /// <param name="roomId"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [HubMethodName("SendMessageToRoom")]
        public async Task SendMessageToRoomAsync([Required] string roomId, [Required] string message)
        {
            var room = await context.chatRooms
                .Where(x => x.ID.Equals(Guid.Parse(roomId))).FirstOrDefaultAsync();

            await Groups.AddToGroupAsync(Context.ConnectionId, room.ID.ToString());
            await UpdateMessagesStatusAsync(roomId);
            await SendMessageAsync(Context.UserIdentifier, message, room);
        }
        
        /// <summary>
        /// this is the method that update the message status isSeen to true
        /// </summary>
        /// <param name="roomId"></param>
        /// <returns></returns>
        private async Task UpdateMessagesStatusAsync(string roomId)
        {
            var listUnseenMessages = await context.chatMessages
                .Where(x => x.roomId.Equals(Guid.Parse(roomId)))
                .Where(x => x.isSeen.Equals(false)).ToListAsync();
            if (!listUnseenMessages.Count.Equals(0))
            {
                foreach (var message in listUnseenMessages)
                {
                    message.isSeen = true;
                }
                context.chatMessages.UpdateRange(listUnseenMessages);
                await context.SaveChangesAsync();
            }
        }

        /// <summary>
        /// represent the method recording message everytime it is called
        /// </summary>
        /// <param name="author"></param>
        /// <param name="message"></param>
        /// <param name="room"></param>
        /// <returns></returns>
        private async Task RecordMessagesAsync(string authorId, string message, Room room)
        {
            var user = await userManager.FindByIdAsync(authorId);

            var newMessage = new Message();
            newMessage.message = message;
            newMessage.userId = authorId;
            newMessage.User = user;
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
            var userFirst = new ApplicationUser();
            try
            {
                userFirst = await userManager.FindByIdAsync(Context.User.FindFirst("ID").Value.ToString());
                if (userFirst is null)
                {
                    userFirst = await userManager.FindByIdAsync(senderId);
                }
            }
            catch (Exception)
            {
                userFirst = await userManager.FindByIdAsync(senderId);
            }
            
            var listRoomfromFirst = await context.chatRooms
                .Include(x => x.users)
                .Where(x => x.users.Contains(userFirst)).ToListAsync();

            var userSecond = await userManager.FindByIdAsync(receiverId);
            var listRoomFromSecond = await context.chatRooms
                .Include(x => x.users)
                .Where(x => x.users.Contains(userSecond)).ToListAsync();

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
        private async Task SendMessageAsync(string senderId, string message, Room room)
        {
            var username = Context.User.Identity.Name;
            await RecordMessagesAsync(senderId, message, room);
            try
            {
                if (!string.IsNullOrEmpty(room.ID.ToString()))
                {
                    if (room.isGroupChat)
                    {
                        await Clients.Group(room.ID.ToString()).Chat(username, message, room.ID.ToString(), roomName: room.roomName);
                    }
                    else
                    {
                        var currentRoomName = string.Empty;
                        foreach (var user in room.users)
                        {
                            currentRoomName = (!user.Id.Equals(Context.UserIdentifier)) ? user.UserName.ToString() : "Name Not found!";
                        }
                        await Clients.Group(room.ID.ToString()).Chat(username, message, room.ID.ToString(), roomName: currentRoomName);
                    }
                }
                else
                {
                    await Clients.All.Chat(user: "System Exception", message: "Room Not Found", "All", roomName: string.Empty);
                }
            }
            catch (Exception ex)
            {
                await Clients.All.Chat("System Exception", ex.Message, "ALL", roomName: string.Empty);
            }
        }
    }
}
