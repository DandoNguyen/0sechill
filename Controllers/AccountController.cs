using _0sechill.Data;
using _0sechill.Dto.Account.Request;
using _0sechill.Dto.Account.Response;
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
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class AccountController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMapper mapper;

        public AccountController(
            ApiDbContext context,
            RoleManager<IdentityRole> roleManager,
            IMapper mapper)
        {
            this.context = context;
            this.roleManager = roleManager;
            this.mapper = mapper;
        }

        [HttpGet, Route("GetAllStaff")]
        public async Task<IActionResult> GetAllStaffAsync()
        {
            var listStaff = await context.ApplicationUser
                .Where(x => x.role.Trim().ToLower().Equals(UserRole.Staffbt))
                .Where(x => x.role.Trim().ToLower().Equals(UserRole.Staffst))
                .ToListAsync();
            var listStaffDto = new List<StaffDto>();
            foreach (var staff in listStaff)
            {
                var staffDto = mapper.Map<StaffDto>(staff);
                staffDto.staffId = staff.Id;
                staffDto.username = staff.UserName;
                listStaffDto.Add(staffDto);
            }
            if (!listStaffDto.Count.Equals(0))
            {
                return Ok(listStaffDto);
            }
            return BadRequest("No Staff found!");
        }

        //Assign Staff in to department

        //Assign Staff to issue

        //Assign Block Manager
        [HttpPost, Route("AssignBlockManager")] 
        public async Task<IActionResult> AssignBlockManagerAsync(AssignBlockManagerDto dto)
        {
            var existUser = await context.ApplicationUser
                .FirstOrDefaultAsync(x => x.Id.Equals(dto.userId));
            if (existUser is null)
                return BadRequest("User not exist");

            var existBlock = await context.blocks
                .FirstOrDefaultAsync(X => X.blockId.Equals(Guid.Parse(dto.blockId)));
            if (existBlock is null)
                return NotFound();

            if (existBlock.blockManager is not null)
            {
                return BadRequest($"Block {existBlock.blockName} has already been assigned to manager {existBlock.blockManager.UserName}");
            }

            if (existUser.role.Equals(UserRole.BlockManager))
            {
                return BadRequest($"User {existUser.UserName} doesn't has appropriate role");
            }

            existBlock.blockManager = existUser;
            context.blocks.Update(existBlock);
            await context.SaveChangesAsync();

            return Ok($"Block {existBlock.blockName} has been assigned to {existUser.UserName}");
        }

        [HttpDelete, Route("RemoveBlockManager")]
        public async Task<IActionResult> RemoveBlockManager(string blockId)
        {
            var existBlock = await context.blocks
                .FirstOrDefaultAsync(x => x.blockId.Equals(Guid.Parse(blockId)));
            if (existBlock is null)
            {
                return NotFound();
            }
            if (existBlock.blockManager is not null)
            {
                return Ok("Block has not been Assigned");
            }
            else
            {
                existBlock.blockManager = null;
                context.blocks.Update(existBlock);
                await context.SaveChangesAsync();
                return Ok("Block Manager Removed");
            }
        }
    }
}
