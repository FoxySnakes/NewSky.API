using NewSky.API.Models;
using System.Security.Claims;

namespace NewSky.API.Services.Interface
{
    public interface ITokenService
    {
        string GenerateToken(User user);
        bool IsTokenValid(string token);
        ClaimsPrincipal GetPrincipalClaims(string token);
    }
}
