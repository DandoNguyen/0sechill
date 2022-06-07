using _0sechill.Dto.UserDto.Request;
using _0sechill.Dto.UserDto.Response;
using _0sechill.Models;
using _0sechill.Models.Dto.UserDto.Request;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AuthController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration,
            IMapper mapper)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.email);
            if (user is null)
            {
                return Unauthorized($"Email {dto.email} not found!");
            }
            else if (user is not null && await userManager.CheckPasswordAsync(user, dto.password))
            {
                var userRole = await userManager.GetRolesAsync(user);

                var authClaim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach(var role in userRole)
                {
                    authClaim.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = GetToken(authClaim);

                ////Get Refresh Token
                //var refreshToken = GetRefreshToken();
                //SetRefreshToken(refreshToken);

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    refreshToken = "Not implemented!"
                });
            }

            return Unauthorized();
        }

        //private async Task<RefreshToken> GetRefreshToken()
        //{

        //}

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegistrationDto dto)
        {
            var existUser = await userManager.FindByEmailAsync(dto.email);
            if(existUser is not null)
                return BadRequest($"Email {dto.email} has been already used!");

            var newUser = mapper.Map<ApplicationUser>(dto);

            var result = await userManager.CreateAsync(newUser, dto.password);
            if(!result.Succeeded)
                return BadRequest(new AuthResponseDto()
                {
                    status = false,
                    message = result.Errors.ToString()
                });

            return Ok(new AuthResponseDto()
            {
                status = true,
                message = "Registration Success\nEmail Verification not implemented"
            });
        }

        private JwtSecurityToken GetToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var token = new JwtSecurityToken(
                issuer: configuration["JWT:ValidIssuer"],
                audience: configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddHours(3),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }
    }
}
