using _0sechill.Data;
using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Models;
using _0sechill.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IFileHandlingService fileService;
        private readonly IConfiguration config;
        private readonly ApiDbContext context;
        private readonly IMapper mapper;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            IFileHandlingService fileService,
            IConfiguration config,
            ApiDbContext context,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.fileService = fileService;
            this.config = config;
            this.context = context;
            this.mapper = mapper;
        }

        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var userList = userManager.Users.ToList();
            if (userList.Count().Equals(0))
            {
                return NoContent();
            }

            var listUserDto = new List<userProfileDto>();
            foreach (var user in userList)
            {
                var userDto = new userProfileDto();
                mapper.Map(user, userDto);
                listUserDto.Add(userDto);
            }
            return Ok(listUserDto);
        }

        [HttpGet]
        [Route("GetUser")]
        public async Task<IActionResult> GetUsers(string userId)
        {
            var user = await userManager.FindByIdAsync(userId);
            if (user is null)
                return new JsonResult("No user found!") { StatusCode = 204 };
            var userDto = mapper.Map<userProfileDto>(user);
            return Ok(userDto);
        }

        [HttpPut]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser([FromForm] UpdateUserDto dto, IFormFile avatar)
        {
            var user = await userManager.FindByIdAsync(dto.userId);
            if (user is null)
                return BadRequest("User is not Found");
            user = mapper.Map<ApplicationUser>(dto);
            
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest($"Error updating User {user.UserName}: {result.Errors}");

            if (avatar is not null)
            {
                var uploadFileResult = await fileService.UploadFile(avatar, user.Id, config["FilePaths:Avatar"]);
                if (!uploadFileResult.isSucceeded)
                {
                    await context.SaveChangesAsync();
                    return new JsonResult($"Update User {user.UserName} success but couldn't upload avatar: {avatar.Name}")
                    {
                        StatusCode = 200
                    };
                }
            }

            await context.SaveChangesAsync();
            return Ok($"User {user.UserName} updated!");
        }
    }
}
