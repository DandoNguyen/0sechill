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
        public async Task SendMessageToUser([Required] string receiverId, [Required] string message, [Required] string Authorization)
        {
            var sender = await tokenService.DecodeTokenAsync(Authorization);

            //Try find exist Room
            var existRoom = await FindExistRoomAsync(sender.Id, receiverId);
            if (existRoom is not null)
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, existRoom.roomName);
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
