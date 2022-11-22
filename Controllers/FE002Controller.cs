using _0sechill.Data;
using _0sechill.Dto;
using _0sechill.Dto.FE002.Request;
using _0sechill.Models;
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
        public static string GenerateRandomPassword(PasswordOptions opts = null)
        {
            if (opts == null) opts = new PasswordOptions()
            {
                RequiredLength = 8,
                RequiredUniqueChars = 4,
                RequireDigit = true,
                RequireLowercase = true,
                RequireNonAlphanumeric = true,
                RequireUppercase = true
            };

            string[] randomChars = new[] {
            "ABCDEFGHJKLMNOPQRSTUVWXYZ",    // uppercase 
            "abcdefghijkmnopqrstuvwxyz",    // lowercase
            "0123456789",                   // digits
            "!@$?_-"                        // non-alphanumeric
        };

            Random rand = new Random(Environment.TickCount);
            List<char> chars = new List<char>();

            if (opts.RequireUppercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[0][rand.Next(0, randomChars[0].Length)]);

            if (opts.RequireLowercase)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[1][rand.Next(0, randomChars[1].Length)]);

            if (opts.RequireDigit)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[2][rand.Next(0, randomChars[2].Length)]);

            if (opts.RequireNonAlphanumeric)
                chars.Insert(rand.Next(0, chars.Count),
                    randomChars[3][rand.Next(0, randomChars[3].Length)]);

            for (int i = chars.Count; i < opts.RequiredLength
                || chars.Distinct().Count() < opts.RequiredUniqueChars; i++)
            {
                string rcs = randomChars[rand.Next(0, randomChars.Length)];
                chars.Insert(rand.Next(0, chars.Count),
                    rcs[rand.Next(0, rcs.Length)]);
            }

            return new string(chars.ToArray());
        }

        /// <summary>
        /// create a profile for employee
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateProfile")]
        public async Task<IActionResult> createProfile(EmployeeInfoDto dto)
        {
            var autoPassword = GenerateRandomPassword();

            resultDto result = new resultDto();
            try
            {
                
                var newEmployee = mapper.Map<ApplicationUser>(dto);
                var nameArray = dto.fullname.Split(" ");
                newEmployee.lastName = nameArray[0];
                newEmployee.firstName = nameArray[1];

                try
                {
                    var registerResult = await userManager.CreateAsync(newEmployee, autoPassword);
                    if (!registerResult.Succeeded)
                    {
                        result.isSuccess = registerResult.Succeeded;
                        result.error += $"{registerResult.Errors}";
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

                result.isSuccess = true;
                result.message = $"Profile Created!\nYour Account: {dto.Email} \nYour Password: {autoPassword}";
                result.error += "";
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
