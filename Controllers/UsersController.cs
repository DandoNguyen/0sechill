using _0sechill.Data;
using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Models;
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
        private readonly IMapper mapper;

        public UsersController(
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            this.userManager = userManager;
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
            return Ok(userList);
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
        public async Task<IActionResult> UpdateUser(UpdateUserDto dto)
        {
            var user = await userManager.FindByIdAsync(dto.userId);
            if (user is null)
                return BadRequest("User is not Found");
            user = mapper.Map<ApplicationUser>(dto);
            
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
                return BadRequest($"Error updating User {user.UserName}: {result.Errors}");
            return Ok($"User {user.UserName} updated!");
        }
    }
}
