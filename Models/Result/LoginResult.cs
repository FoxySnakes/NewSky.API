using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class LoginResult
    {
        public UserDto? User { get; set; } = null;
        public bool IsSuccess { get; set; }
        public bool IsLocked { get; set; } = false;
        public TimeSpan LockoutEnd { get; set; } = TimeSpan.Zero;
        public bool IsBanned { get; set; } = false;
        public TimeSpan BanishmentEnd { get; set; } =  TimeSpan.Zero;
        public string Token { get; set; }
    }
}
