using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class LoginResult
    {
        public UserDto? User { get; set; } = null;
        public bool IsSuccess { get; set; }
        public bool IsLocked { get; set; } = false;
        public DateTime? LockoutEnd { get; set; } = null;
        public bool IsBanned { get; set; } = false;
        public DateTime? BanishmentEnd { get; set; } = null;
        public string Token { get; set; }
    }
}
