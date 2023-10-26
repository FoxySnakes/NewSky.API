using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class LoginResult
    {
        public UserDto? User { get; set; } = null;
        public bool IsSuccess { get; set; }
        public bool IsLocked { get; set; } = false;
        public bool IsBanned { get; set; } = false;
        public bool RequiresTwoFactor { get; set; } = false;
        public string Token { get; set; }
    }
}
