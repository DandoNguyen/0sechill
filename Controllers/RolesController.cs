using _0sechill.Data;
using _0sechill.Models;
using _0sechill.Static;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin")]
    public class RolesController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public RolesController(
            ApiDbContext context,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpGet, Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUserAsync()
        {
            var listUser = await userManager.Users.ToListAsync();
            return Ok(listUser);
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
    }
}
