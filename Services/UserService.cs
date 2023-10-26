using Microsoft.AspNetCore.Identity;
using NewSky.API.Models;
using NewSky.API.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Net.Http.Json;

namespace NewSky.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;
        private readonly HttpClient _httpClient;

        public UserService(IHttpContextAccessor httpContextAccessor,
                           UserManager<User> userManager,
                           HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _httpClient = httpClient;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            return await _userManager.FindByNameAsync(_httpContextAccessor.HttpContext.User.Identity.Name);
        }

        public string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext.User.Identity.Name;
        }

        public string GetCurrentUserIp()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var userIp = httpContext.Connection.RemoteIpAddress?.ToString();
#if DEBUG
            userIp = "77.133.249.114";
#endif
            return userIp;
        }

        public async Task<string> GetUserUUIDAsync(string username)
        {
            var response = await _httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);
            return jsonContent["id"].ToString();
        }
    }
}
