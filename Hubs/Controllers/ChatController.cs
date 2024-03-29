﻿using _0sechill.Data;
using _0sechill.Dto.FE001.Response;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Dto.Response;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Services;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Hubs.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ChatController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly ITokenService tokenService;
        //private readonly IHubContext hubContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(
            ApiDbContext context,
            ITokenService tokenService,
            //IHubContext hubContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.tokenService = tokenService;
            //this.hubContext = hubContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// represent the method that get the lastest message of a room
        /// </summary>
        /// <param name="roomId">Id of the mentioned room</param>
        /// <returns>message full text of the latest message</returns>
        [HttpGet, Route("GetLatestMesage")]
        public async Task<IActionResult> getLatestMessageOfRoom(string roomId)
        {
            var lastestMessage = await context.chatMessages
                .Where(x => x.roomId.Equals(Guid.Parse(roomId)))
                .OrderByDescending(x => x.createdDateTime)
                .FirstOrDefaultAsync();
            return Ok(lastestMessage.message);
        }

        /// <summary>
        /// represent the method getting all the chat rooms of a user
        /// </summary>
        /// <returns>List of Rooms of user</returns>
        [HttpGet, Route("GetAllMyRoomChat")]
        public async Task<IActionResult> GetAllMyRoomChatAsync()
        {
            var user = await userManager.FindByIdAsync(User.FindFirst("ID").Value);
            var listRoom = await context.chatRooms
                .Include(x => x.users)
                .Where(x => x.users.Contains(user))
                .ToListAsync();
            if (listRoom.Count.Equals(0))
            {
                return NoContent();
            }
            var listRoomDto = new List<RoomDto>();
            foreach (var room in listRoom)
            {
                var roomDetailObject = DetermineRoomNameAsync(room, user);
                room.roomName = roomDetailObject.roomName;
                var roomDto = mapper.Map<RoomDto>(room);
                roomDto.receiverId = roomDetailObject.receiverId;
                listRoomDto.Add(roomDto);
            }
            return Ok(listRoomDto);
        }

        /// <summary>
        /// represent the method deciding which name to represent each type of chat room
        /// </summary>
        /// <param name="room"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        private dynamic DetermineRoomNameAsync(Room room, ApplicationUser user)
        {
            var receiverId = String.Empty;
            if (room.roomName is null)
            {
                foreach (var userItem in room.users)
                {
                    if (userItem != user)
                    {
                        room.roomName = userItem.UserName;
                        receiverId = userItem.Id;
                    }
                }
            }
            return new { 
                roomName = room.roomName.ToString(), 
                receiverId = receiverId
            };
        }

        /// <summary>
        /// represent the method loading older message available for that room
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet, Route("LoadOldMessage")]
        public async Task<IActionResult> LoadOldMessageAsync(string roomId, DateTime SearchDate, bool isSearchByDate)
        {
            var listMessage = context.chatMessages.Where(x => x.roomId.Equals(Guid.Parse(roomId))).AsQueryable();

            if (isSearchByDate)
            {
                listMessage = listMessage.Where(x => x.createdDateTime <= SearchDate);
            }

            var listResult = await listMessage.OrderBy(x => x.createdDateTime).Take(10).ToListAsync();

            //var listMessage = await context.chatMessages
            //    .Include(x => x.User)
            //    .Where(x => x.roomId.Equals(Guid.Parse(roomId)))
            //    .Where(x => x.createdDateTime <= SearchDate)
            //    .OrderBy(x => x.createdDateTime).Take(10)
            //    .ToListAsync();

            if (listResult.Count.Equals(0))
            {
                return NoContent();
            }

            var listMessageDto = new List<MessageResponseDto>();
            foreach (var message in listResult)
            {
                var MessageDto = mapper.Map<MessageResponseDto>(message);
                listMessageDto.Add(MessageDto);
            }
            return Ok(listMessageDto);
        }

        /// <summary>
        /// represent the method adding new room for group chat
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HttpPost, Route("AddNewRoom")]
        public async Task<IActionResult> AddRoomAsync(AddNewRoomDto dto, [FromHeader] string Authorization)
        {
            var newRoom = new Room();
            newRoom.roomName = dto.roomName;

            var adminUser = await tokenService.DecodeTokenAsync(Authorization);
            newRoom.groupAdmin = adminUser.Id;

            foreach (var userId in dto.listUserId)
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user is not null)
                {
                    newRoom.users.Add(user);
                }
            }

            try
            {
                await context.chatRooms.AddAsync(newRoom);
            }
            catch (Exception ex)
            {
                return BadRequest($"Room Creation Error: {ex.Message}");
            }

            await context.SaveChangesAsync();
            return Ok($"Room {newRoom.roomName} created successfully!");
        }

        [HttpGet, Route("GetAllChatUser")]
        public async Task<IActionResult> GetAllChatUser()
        {
            var loggedInUser = await userManager.FindByIdAsync(User.FindFirst("ID").Value);
            if (loggedInUser is null)
            {
                return Unauthorized("Token Invalid");
            }

            var listUserDto = new List<UserDto>();
            var listResult = await userManager.Users.ToListAsync();
            foreach (var user in listResult)
            {
                var listRole = await userManager.GetRolesAsync(user);
                if (listRole.Contains(UserRole.Citizen))
                {
                    var userDto = new UserDto();
                    userDto = mapper.Map<UserDto>(user);
                    listUserDto.Add(userDto);
                }
            }

            return Ok(listUserDto);
        }
    }
}
