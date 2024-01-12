using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewSky.API.Models.Db;
using NewSky.API.Services.Interface;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;

namespace NewSky.API.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IRepository<User> _userRepository;
        private readonly HttpClient _httpClient;

        public UserService(IHttpContextAccessor httpContextAccessor,
                           IRepository<User> userRepository,
                           HttpClient httpClient)
        {
            _httpContextAccessor = httpContextAccessor;
            _userRepository = userRepository;
            _httpClient = httpClient;
        }
        public async Task<User> GetCurrentUserAsync()
        {
            var x = _httpContextAccessor.HttpContext.User.Identity.Name;
            return await _userRepository.Query().Include(x => x.Roles).ThenInclude(x => x.Role)
                                                .Include(x => x.Permissions).ThenInclude(x => x.Permission)
                                                .FirstOrDefaultAsync(x => x.UserName == _httpContextAccessor.HttpContext.User.Identity.Name);
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
            userIp = "91.173.177.105";
#endif
            return userIp;
        }

        public async Task<string> GetUserUUIDAsync(string username)
        {
            var response = await _httpClient.GetAsync($"https://api.mojang.com/users/profiles/minecraft/{username}");
            if(response.StatusCode == HttpStatusCode.NotFound)
            {
                return string.Empty;
            }

            var content = await response.Content.ReadAsStringAsync();
            var jsonContent = JObject.Parse(content);

            var x = jsonContent["id"].ToString();
            var uuid = x.Insert(8, "-").Insert(13, "-").Insert(18, "-").Insert(23, "-");
            return uuid;
        }
    }
}
