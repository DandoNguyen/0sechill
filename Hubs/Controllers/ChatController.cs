using _0sechill.Data;
using _0sechill.Hubs.Dto;
using _0sechill.Hubs.Dto.Response;
using _0sechill.Hubs.Model;
using _0sechill.Models;
using _0sechill.Services;
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
        private readonly IHubContext hubContext;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public ChatController(
            ApiDbContext context,
            ITokenService tokenService,
            IHubContext hubContext,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.tokenService = tokenService;
            this.hubContext = hubContext;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// represent the method getting all the chat rooms of a user
        /// </summary>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllMyRoomChat")]
        public async Task<IActionResult> GetAllMyRoomChatAsync([FromHeader] string Authorization)
        {
            var user = await tokenService.DecodeTokenAsync(Authorization);
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
                room.roomName = DetermineRoomNameAsync(room, user);
                var roomDto = mapper.Map<RoomDto>(room);
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
        private string DetermineRoomNameAsync(Room room, ApplicationUser user)
        {
            if (room.roomName is null)
            {
                foreach (var userItem in room.users)
                {
                    if (userItem != user)
                    {
                        room.roomName = userItem.UserName;
                    }
                }
            }
            return room.roomName.ToString();
        }

        /// <summary>
        /// represent the method loading older message available for that room
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpGet, Route("LoadOldMessage")]
        public async Task<IActionResult> LoadOldMessageAsync(MessageRequestDto dto)
        {
            var listMessage = await context.chatMessages
                .Include(x => x.User)
                .Where(x => x.roomId.Equals(dto.roomId))
                .Where(x => x.createdDateTime <= dto.SearchDate)
                .OrderByDescending(x => x.createdDateTime).Take(10)
                .ToListAsync();

            if (listMessage.Count.Equals(0))
            {
                return NoContent();
            }

            var listMessageDto = new List<MessageResponseDto>();
            foreach (var message in listMessage)
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
    }
}
