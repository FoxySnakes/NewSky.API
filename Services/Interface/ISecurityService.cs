using NewSky.API.Models.Db;

namespace NewSky.API.Services.Interface
{
    public interface ISecurityService
    {
        string GenerateToken(User user);
        bool IsTokenValid(string token);
        string HashPassword(string password);
    }
}
