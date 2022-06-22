using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Models;
using _0sechill.Models.Dto.UserDto.Request;
using _0sechill.Services;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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

        public AuthController(
            UserManager<ApplicationUser> userManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.tokenService = tokenService;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAsync([FromBody] RegistrationDto dto)
        {
            if (ModelState.IsValid)
            {
                var existingUser = await userManager.Users
                    .FirstOrDefaultAsync(x => x.Email.Equals(dto.email));
                if (existingUser is not null)
                    return BadRequest(new AuthResponseDto()
                    {
                        success = false,
                        message =
                        {
                            $"Email {dto.email} has been registered!"
                        }
                    });

                var newUser = mapper.Map<ApplicationUser>(dto);

                var isCreated = await userManager.CreateAsync(newUser, dto.password);

                if (isCreated.Succeeded)
                    return Ok(new AuthResponseDto()
                    {
                        success = true,
                        message =
                        {
                            $"User {dto.UserName} registered Successfully!"
                        }
                    });
                else
                    return new JsonResult(new AuthResponseDto()
                    {
                        success = false,
                        message = isCreated.Errors.Select(x => x.Description).ToList()
                    }) { StatusCode = 500 };
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
    }
}
