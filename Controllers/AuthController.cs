using _0sechill.Dto.Account.Request;
using _0sechill.Dto.MailDto;
using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Models;
using _0sechill.Models.Dto.UserDto.Request;
using _0sechill.Services;
using _0sechill.Static;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OfficeOpenXml.FormulaParsing.LexicalAnalysis;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _0sechill.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ITokenService tokenService;
        private readonly IMapper mapper;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IMailService mailService;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IMapper mapper, 
            RoleManager<IdentityRole> roleManager,
            IMailService mailService)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
            this.roleManager = roleManager;
            this.mailService = mailService;
        }

        /// <summary>
        /// represent the method creating staff account for the admin user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost, Route("CreateStaffAccount")]
        [Authorize(Roles = "admin", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> CreateAccountAsync(CreateStaffAccountDto dto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.FindByEmailAsync(dto.email);
                if (existingUser is not null)
                    return BadRequest(new AuthResponseDto()
                    {
                        success = false,
                        message =
                        {
                            $"Email {dto.email} has been used"
                        }
                    });
                var newUser = mapper.Map<ApplicationUser>(existingUser);
                newUser.role = UserRole.Staffbt;
                var isCreated = await userManager.CreateAsync(newUser);
                if (isCreated.Succeeded)
                {
                    return Ok(new AuthResponseDto()
                    {
                        success = true,
                        message =
                        {
                            $"Staff {dto.username} account created"
                        }
                    });
                }
                else
                    return new JsonResult(new AuthResponseDto()
                    {
                        success = false,
                        message = isCreated.Errors.Select(x => x.Description).ToList()
                    })
                    { StatusCode = 500 };
            }
            return BadRequest(new AuthResponseDto()
            {
                success = false,
                message =
                {
                    "Invald Payload!"
                }
            });
        }

        /// <summary>
        /// represent the method register new account
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationDto dto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.Users
                    .FirstOrDefaultAsync(x => x.Email.Equals(dto.Email));
                if (existingUser is not null)
                    return BadRequest(new AuthResponseDto()
                    {
                        success = false,
                        message =
                        {
                            $"Email {dto.Email} has been registered!"
                        }
                    });

                var newUser = mapper.Map<ApplicationUser>(dto);

                var isCreated = await userManager.CreateAsync(newUser, dto.password);

                if (isCreated.Succeeded) {
                    var citizenRole = new IdentityRole(UserRole.Citizen.ToString());
                    if (!await roleManager.RoleExistsAsync(UserRole.Citizen.ToString().ToLower()))
                    {
                        await roleManager.CreateAsync(citizenRole);
                    }

                    newUser = await userManager.FindByEmailAsync(dto.Email);
                    await userManager.AddToRoleAsync(newUser, citizenRole.Name);

                    return Ok(new AuthResponseDto()
                    {
                        success = true,
                        message =
                        {
                            $"User {dto.UserName} registered Successfully!"
                        }
                    });
                }
                else
                return new JsonResult(new AuthResponseDto()
                {
                    success = false,
                    message = isCreated.Errors.Select(x => x.Description).ToList()
                })
                { StatusCode = 500 };
            }
            return BadRequest(new AuthResponseDto()
            {
                success = false,
                message =
                {
                    "Invald Payload!"
                }
            });
        }

        /// <summary>
        /// represent the method register new account for admin
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdminAsync([FromBody] RegistrationDto dto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.Users
                    .FirstOrDefaultAsync(x => x.Email.Equals(dto.Email));
                if (existingUser is not null)
                    return BadRequest(new AuthResponseDto()
                    {
                        success = false,
                        message =
                        {
                            $"Email {dto.Email} has been registered!"
                        }
                    });

                var newUser = mapper.Map<ApplicationUser>(dto);

                var isCreated = await userManager.CreateAsync(newUser, dto.password);

                if (isCreated.Succeeded)
                {
                    var adminRole = new IdentityRole(UserRole.Admin.ToString());
                    if (!await roleManager.RoleExistsAsync(UserRole.Admin.ToString().ToLower()))
                    {
                        await roleManager.CreateAsync(adminRole);
                    }

                    newUser = await userManager.FindByEmailAsync(dto.Email);
                    if (newUser is not null)
                    {
                        await userManager.AddToRoleAsync(newUser, adminRole.Name);
                    }
                    return Ok(new AuthResponseDto()
                    {
                        success = true,
                        message =
                        {
                            $"Admin {dto.UserName} registered Successfully!"
                        }
                    });
                }
                    
                else
                    return new JsonResult(new AuthResponseDto()
                    {
                        success = false,
                        message = isCreated.Errors.Select(x => x.Description).ToList()
                    })
                    { StatusCode = 500 };
            }
            return BadRequest(new AuthResponseDto()
            {
                success = false,
                message =
                {
                    "Invald Payload!"
                }
            });
        }

        /// <summary>
        /// represent the method getting profile details from login user
        /// </summary>
        /// <param name="Authorization"></param>
        /// <returns></returns>
        [HttpGet, Route("GetProfile")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetProfileAsync()
        {
            var user = await userManager.FindByIdAsync(User.FindFirst("ID").Value);
            if (user is null)
                return BadRequest("Token invalid");

            var ListRole = await userManager.GetRolesAsync(user);
            return new JsonResult(new
            {
                username = user.UserName,
                email = user.Email,
                role = ListRole
            })
            { StatusCode = 200 };
        }

        /// <summary>
        /// represent the method login 
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> LoginAsync([FromBody] LoginDto dto)
        {
            var existingUser = await userManager.FindByEmailAsync(dto.email);

            if (existingUser is null)
            {
                return BadRequest(new AuthResponseDto()
                {
                    success = false,
                    message =
                    {
                        "Email is not exist!"
                    }
                });
            }

            //check for password
            var isPasswordCorrect = await userManager.CheckPasswordAsync(existingUser, dto.password);
            if (!isPasswordCorrect)
            {
                return BadRequest(new AuthResponseDto()
                {
                    success = false,
                    message =
                    {
                        "Password is not correct!"
                    }
                });
            }

            //Generate Token
            var jwtToken = await tokenService.GenerateJwtToken(existingUser);
            return Ok(new AuthResponseDto()
            {
                success = true,
                token = jwtToken
            });
        }

        /// <summary>
        /// this is for reseting password
        /// </summary>
        /// <param name="email">email of the account that needs reset</param>
        /// <returns>Http Response</returns>
        [HttpPost, Route("ForgotPassword")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword([FromBody] [Required] [EmailAddress] string email)
        {
            var user = await userManager.FindByNameAsync(email);
            if (user is null)
            {
                return BadRequest("Email does not exist");
            }

            //reset pass token
            var newPassToken = await userManager.GeneratePasswordResetTokenAsync(user);
            var passwordResetLink = Url.Action(nameof(resetPasswordAsync), "Auth", new { email = user.Email, newPassToken }, Request.Scheme);

            //inform by email to user
            var mailContent = new MailContent()
            {
                ToEmail = user.Email,
                Subject = "Password Reset",
                Body = $"Please click the link below to confirm request new password for your account {user.Email}:\n\n{passwordResetLink}\n\nThis link will be expired within two hours"
            };
            await mailService.SendMailAsync(mailContent);
            return Ok($"Please check you email ({user.Email}) inbox ");
        }

        /// <summary>
        /// reset password endpoints
        /// </summary>
        /// <param name="email"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("ResetPassword")]
        public async Task<IActionResult> resetPasswordAsync(string email, string token)
        {
            var user = await userManager.FindByEmailAsync(email);
            var newPassword = Guid.NewGuid().ToString();

            var resetPassResult = await userManager.ResetPasswordAsync(user, token, newPassword);
            if (resetPassResult.Succeeded)
            {
                return Ok($"new Pass: {newPassword}");
            }

            return Ok("Error");
        }
    }
}
