using _0sechill.Data;
using _0sechill.Dto.Account.Response;
using _0sechill.Dto.Department.Request;
using _0sechill.Models;
using _0sechill.Models.Account;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class DepartmentController : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public DepartmentController(
            ApiDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        /// <summary>
        /// getting a list of available departments
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllDepartment")]
        public async Task<IActionResult> GetAllDeptAsync()
        {
            var listDepartment = await context.departments.ToListAsync();
            if (!listDepartment.Any())
                return Ok("No Department Found");
            return Ok(listDepartment);
        }

        /// <summary>
        /// creating new department
        /// </summary>
        /// <param name="departmentName"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateDept")]
        public async Task<IActionResult> CreateDeptpartment([FromBody] string departmentName)
        {
            var newDept = new Department();
            newDept.departmentId = Guid.NewGuid();
            newDept.departmentName = departmentName.Trim().ToLower();
            if (ModelState.IsValid)
            {
                await context.departments.AddAsync(newDept);
                await context.SaveChangesAsync();
                return Ok($"New Dept created with name: {newDept.departmentName}");
            }
            return BadRequest("Error in adding new department");
        }

        /// <summary>
        /// assigning new staff to a specific department
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("AssignUserToDept")]
        public async Task<IActionResult> AssignUserAsync(UserIdDeptIdDto dto)
        {
            var existUser = await userManager.FindByIdAsync(dto.userId);
            if (existUser is null)
                return BadRequest("User not Found");

            var existDept = await context.departments.FirstOrDefaultAsync(x => x.departmentId.Equals(Guid.Parse(dto.deptId)));
            if (existDept is null)
                return BadRequest("Department not found");

            if (!existUser.role.Equals(UserRole.Citizen))
            {
                try
                {
                    existDept.users.Add(existUser);
                    await context.SaveChangesAsync();
                    return Ok($"User {existUser.UserName} added to department {existDept.departmentName}");
                }
                catch (Exception ex)
                {
                    return BadRequest($"Error in Adding user {existUser.UserName} to department {existDept.departmentName}:\n{ex.Message}");
                }
            }
            return BadRequest($"User {existUser.UserName} does not have valid Role");
        }

        /// <summary>
        /// getting list of available users in a specific department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <returns></returns>
        [HttpGet, Route("GetUsersInDept")]
        public async Task<IActionResult> GetAllUserDeptAsync([FromBody] [Required] string departmentId) 
        {
            var existDept = await context.departments.FirstOrDefaultAsync(x => x.departmentId.Equals(Guid.Parse(departmentId)));
            if (existDept is null)
                return BadRequest("Department not found");

            var listStaffDto = new List<StaffDto>();
            foreach (var staff in existDept.users)
            {
                var staffDto = mapper.Map<StaffDto>(staff);
                listStaffDto.Add(staffDto);
            }
            return Ok(listStaffDto);
        }
    }
}
