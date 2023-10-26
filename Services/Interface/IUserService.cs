using NewSky.API.Models;

namespace NewSky.API.Services.Interface
{
    public interface IUserService
    {
        Task<User> GetCurrentUserAsync();
        string GetCurrentUserIp();

        string GetCurrentUserName();
        Task<string> GetUserUUIDAsync(string username);
    }
}
