using _0sechill.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _0sechill.Services.Class
{
    public class TokenService : ITokenService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public TokenService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }
        public async Task<ApplicationUser> DecodeTokenAsync(string Authorization)
        {
            if (Authorization is null)
            {
                return null;
            }

            string[] Collection = Authorization.Split(" ");

            //Decode the token
            var stream = Collection[1];
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(stream);
            var tokenS = jsonToken as JwtSecurityToken;

            //get the user
            var email = tokenS.Claims.First(claim => claim.Type == "email").Value;
            var user = await userManager.FindByEmailAsync(email);

            //return the user
            return user;
        }

        public async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var secretKey = Encoding.UTF8.GetBytes(configuration["JWT:Secret"]);

            var issuer = configuration["JWT:ValidIssuer"];
            var audience = configuration["JWT:ValidAudience"];
            var securityKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

            var jwtTokenHandler = new JwtSecurityTokenHandler();

            var claims = await GetAllValidclaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                Audience = audience,
                Issuer = issuer,
                SigningCredentials = credentials
            };

            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = jwtTokenHandler.WriteToken(token);
            return jwtToken;
        }

        private async Task<List<Claim>> GetAllValidclaims(ApplicationUser user)
        {
            //var options = new IdentityOptions();

            //Get basic Claims
            var claims = new List<Claim> {
                new Claim("ID", user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            //Getting the claims that we have assigned to the user
            var userClaims = await userManager.GetClaimsAsync(user);
            claims.AddRange(userClaims);

            //Get the user role and add to the user
            var userRoles = await userManager.GetRolesAsync(user);
            if (!userRoles.Count.Equals(0))
            {
                foreach (var userRole in userRoles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, userRole));

                    var role = await roleManager.FindByNameAsync(userRole);

                    if (role is not null)
                    {
                        var listRoleClaims = await roleManager.GetClaimsAsync(role);
                        foreach (var roleClaim in listRoleClaims)
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }
            }

            return claims;
        }
    }
}
