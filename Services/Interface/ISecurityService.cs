using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using System.Security.Claims;

namespace NewSky.API.Services.Interface
{
    public interface ISecurityService
    {
        string GenerateToken(User user);
        bool IsTokenValid(string token);
        string HashPassword(string password);
    }
}
