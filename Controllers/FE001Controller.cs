﻿using _0sechill.Data;
using _0sechill.Dto.FE001.Model;
using _0sechill.Dto.FE001.Request;
using _0sechill.Dto.FE001.Response;
using _0sechill.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace _0sechill.Controllers
{
    /// <summary>
    /// Controller for List User
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class FE001Controller : ControllerBase
    {
        private readonly ApiDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public FE001Controller(
            ApiDbContext context, IMapper mapper, 
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        #region Endpoints

        /// <summary>
        /// This method is to enable or disable a user
        /// </summary>
        /// <param name="UserID"></param>
        /// <param name="EnableOrDisable"></param>
        /// <returns></returns>
        [HttpPost, Route("EnableOrDisableUser")]
        public async Task<IActionResult> ActivateUser(string UserID, bool EnableOrDisable)
        {
            //Prevent Frontend dipshit from sending his own logged in UserID to this endpoint
            var loggedInUser = await userManager.FindByIdAsync(this.User.FindFirst("ID").Value);
            if (loggedInUser is not null && loggedInUser.Id.Equals(UserID))
            {
                return BadRequest("Djtmemay Nam :) del phai ID cua m");
            }

            var user = await userManager.FindByIdAsync(UserID);
            if (user is null)
            {
                return BadRequest("User not found");
            }

            if (EnableOrDisable)
            {
                if (user.isActive)
                {
                    return Ok("User is already active");
                } 
                else
                {
                    user.isActive = false;
                    await context.SaveChangesAsync();
                    return Ok("User Disabled");
                }
            }
            else
            {
                if (!user.isActive)
                {
                    return Ok("User is already disabled");
                } 
                else
                {
                    user.isActive = true;
                    await context.SaveChangesAsync();
                    return Ok("User Avtivated");
                }
            }
            
        }

        /// <summary>
        /// return all user as array
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetAllUser(bool getActive)
        {
            var listResults = new List<UserDto>();
            var listUsers = await userManager.Users.ToListAsync();
            foreach (var user in listUsers)
            {
                if (user.isActive.Equals(getActive))
                {
                    var userDto = new UserDto();
                    userDto = mapper.Map<UserDto>(user);
                    userDto.roleName = (List<string>)await userManager.GetRolesAsync(user);
                    listResults.Add(userDto);
                }
            }
            return Ok(listResults);
        }

        /// <summary>
        /// Search by filter function
        /// </summary>
        /// <param name="dto">Param of filter</param>
        /// <returns>List of filtered User</returns>
        [HttpGet, Route("SearchUser")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> GetByFilter(SearchFilterDto dto)
        {
            searchFilterResultDto response = new searchFilterResultDto();
            List<ApplicationUser> listUserResult = new List<ApplicationUser>();
            var searchQuery = userManager.Users.AsQueryable();
            searchQuery = searchQuery.Where(user => user.firstName.Contains(dto.nameString) || user.lastName.Contains(dto.nameString));

            if (dto.hasGenderCheck)
            {
                searchQuery = searchQuery.Where(user => user.isMale.Equals(dto.isMale));
            }

            if (dto.hasAgeCheck)
            {
                searchQuery = searchQuery.Where(user => user.age >= dto.ageFrom && user.age <= dto.ageTo);
            }

            try
            {
                listUserResult = await searchQuery.ToListAsync();
                response.isSucceed = true;
            }
            catch (Exception ex)
            {
                response.error += $"\n{ex.Message}";
                response.isSucceed = false;
                
            }

            if (dto.hasRoleCheck && listUserResult.Any())
            {
                if (dto.roleID is not null) 
                {
                    var role = await roleManager.FindByIdAsync(dto.roleID);
                    foreach (var user in listUserResult)
                    {
                        var listRoles = await userManager.GetRolesAsync(user);

                        if (listRoles.Contains(role.Name))
                        {
                            var userModel = mapper.Map<FE001UserModel>(user);
                            userModel.listRoles = (List<string>) listRoles;
                            response.result.Add(userModel);
                        }
                    }
                }

                else
                {
                    var role = await roleManager.FindByNameAsync(dto.roleName);

                    foreach (var user in listUserResult)
                    {
                        var listRoles = await userManager.GetRolesAsync(user);

                        if (listRoles.Contains(role.Name))
                        {
                            var userModel = mapper.Map<FE001UserModel>(user);
                            userModel.listRoles = (List<string>)listRoles;
                            response.result.Add(userModel);
                        }
                    }
                }
            }

            else if (listUserResult.Any())
            {
                var listUserModel = new List<FE001UserModel>();
                foreach (var user in listUserResult)
                {
                    var listRoles = await userManager.GetRolesAsync(user);

                    var userModel = mapper.Map<FE001UserModel>(user);
                    userModel.listRoles = (List<string>)listRoles;
                    listUserModel.Add(userModel);
                }

                response.result = listUserModel;
                response.isSucceed = true;
            }

            return Ok(response);
        }

        #endregion
    }
}
