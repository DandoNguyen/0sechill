using _0sechill.Models;
using System.Security.Claims;

namespace _0sechill.Services
{
    public interface ITokenService
    {
        Task<ApplicationUser> DecodeTokenAsync(string token);
        Task<string> GenerateJwtToken(ApplicationUser user);
    }
}
