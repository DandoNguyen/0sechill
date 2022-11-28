using _0sechill.Data;
using _0sechill.Dto.Account.Response;
using _0sechill.Dto.Role.Request;
using _0sechill.Models;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "admin, blockManager")]
    public class RolesController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.mapper = mapper;
            this.roleManager = roleManager;
        }

        [HttpPost, Route("TestAdminAccount")]
        public async Task<IActionResult> AddAdminAsync(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return BadRequest("Email not Exist");
            }
            await userManager.AddToRoleAsync(user, UserRole.Admin);
            return Ok($"Admin assigned to {user.Email}");
        }

        [HttpGet, Route("GetAllStaff")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var listUser = await userManager.Users.ToListAsync();
            var listStaffDto = new List<StaffDto>();
            foreach (var user in listUser)
            {
                if (user.role.Equals(UserRole.Staffbt))
                {
                    var staffDto = mapper.Map<StaffDto>(user);
                    listStaffDto.Add(staffDto);
                }
            }
            return Ok(listStaffDto);
        }

        [HttpGet, Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            await ImportHardCodedRoles();
            var listRoles = await roleManager.Roles.ToListAsync();
            return Ok(listRoles);
        }

        private async Task ImportHardCodedRoles()
        {
            var staticRoles = UserRole.GetFields();
            foreach (var role in staticRoles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    continue;
                }
                await roleManager.CreateAsync(new IdentityRole(role));
            }
            await context.SaveChangesAsync();
        }

        [HttpPost, Route("CreateRoles")]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var existRole = await roleManager.RoleExistsAsync(roleName.ToLower());
            if (existRole)
            {
                return BadRequest($"{roleName} has already existed");
            }
            else
            {
                var addRoleResult = await roleManager.CreateAsync(new IdentityRole(roleName.Trim().ToLower()));
                if (addRoleResult.Succeeded)
                {
                    return Ok($"{roleName} has been created");
                }
                return BadRequest("Error in creating Role");
            }
        }

        //Assign Role
        [HttpPost, Route("AssignRole")]
        public async Task<IActionResult> AssignRoleAsync(AssignRoleDto dto) 
        {
            var existUser = await userManager.FindByIdAsync(dto.userId);
            if (existUser is null)
                return BadRequest("User not Found");

            var roleResult = await roleManager.RoleExistsAsync(dto.roleName);
            if (!roleResult)
                return BadRequest("Role not found");

            try
            {
                await userManager.AddToRoleAsync(existUser, dto.roleName);
                await context.SaveChangesAsync();
                return Ok($"User {existUser.UserName} has been assigned with role {dto.roleName}");
            }
            catch (Exception ex)
            {
                return new JsonResult($"Assign Role process returned an error:\n{ex.Message}");
            }
        }

        [HttpGet, Route("GetUserRole")]
        public async Task<IActionResult> GetUserRoleAsync([FromBody] string userId)
        {
            var existUser = await userManager.FindByIdAsync(userId);
            if (existUser is null)
                return BadRequest("User Not Found");

            var roles = await userManager.GetRolesAsync(existUser);
            if (!roles.Any())
                return Ok($"User {existUser.UserName} hasn't been assigned to any roles");
            return Ok(roles);
        }

        [HttpDelete, Route("RemoveRole")]
        public async Task<IActionResult> RemoveRoleAsync(RemoveRoleDto dto)
        {
            var existUser = await userManager.FindByIdAsync(dto.userID);
            if (existUser is null)
                return BadRequest("User Not Found");
            
            var listResult = new List<string>();
            foreach (var role in dto.listRoles)
            {
                if (await roleManager.RoleExistsAsync(role))
                {
                    await userManager.RemoveFromRoleAsync(existUser, role);
                    listResult.Add($"Role {role} removed");
                }
            }
            await context.SaveChangesAsync();
            return Ok(listResult);
        }
    }
}
