using _0sechill.Data;
using _0sechill.Dto;
using _0sechill.Dto.FE002.Request;
using _0sechill.Models;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    /// <summary>
    /// Controller for Creating Employee Profile
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class FE002Controller : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public FE002Controller(
            ApiDbContext context, 
            IMapper mapper, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region endpoints

        /// <summary>
        /// get all roles in the system
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllRoles")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            var listRoles = await roleManager.Roles.ToListAsync();
            return Ok(listRoles);
        }

        /// <summary>
        /// Generates a Random Password
        /// respecting the given strength requirements.
        /// </summary>
        /// <param name="opts">A valid PasswordOptions object
        /// containing the password strength requirements.</param>
        /// <returns>A random password</returns>
        public static string GenerateRandomPassword(int PasswordLength)
        {
            string _allowedChars = "0123456789abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ";
            Random randNum = new Random();
            char[] chars = new char[PasswordLength];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < PasswordLength; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }

        /// <summary>
        /// create a profile for employee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateProfile")]
        public async Task<IActionResult> createProfile(EmployeeInfoDto dto)
        {
            var autoPassword = GenerateRandomPassword(20);

            resultDto result = new resultDto();
            try
            {
                var newEmployee = mapper.Map<ApplicationUser>(dto);
                var nameArray = dto.fullname.Split(" ");
                newEmployee.lastName = nameArray[0];
                newEmployee.firstName = String.Empty;

                for (int i = 1; i < nameArray.Length; i++)
                {
                    newEmployee.firstName += nameArray[i];
                }
                

                try
                {
                    var registerResult = await userManager.CreateAsync(newEmployee, autoPassword);

                    if (!dto.isStaff)
                    {
                        var role = await roleManager.FindByIdAsync(dto.roleID);
                        if (!role.Name.Equals(UserRole.Citizen))
                        {
                            return BadRequest($"{role.Name} cannot be assigned to a resident Account");
                        }
                    }

                    if (!registerResult.Succeeded)
                    {
                        result.isSuccess = registerResult.Succeeded;
                        result.error += $"{registerResult.ToString}";
                    } 
                    else
                    {
                        result.isSuccess = true;
                        result.message = $"Profile Created!\nYour Account: {dto.Email} \nYour Password: {autoPassword}";
                        result.error += "";
                    }
                }
                catch (Exception ex)
                {
                    result.error += $"\n{ex.Message}";
                }

                if (!await AssignEmployeeToRole(newEmployee, dto.roleID))
                {
                    result.error += "\nCould not add Role";
                }
            }
            catch (Exception ex)
            {
                result.isSuccess = false;
                result.message += $"\nWith error";
                result.error += $"\n{ex.Message}";
            }

            return Ok(result);
        }


        private async Task<bool> AssignEmployeeToRole(ApplicationUser employee, string roleID)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(roleID);
                await userManager.AddToRoleAsync(employee, role.Name);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion
    }
}
