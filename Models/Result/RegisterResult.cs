using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class RegisterResult
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public List<IdentityError> Errors { get; set; } = new List<IdentityError>();
        public bool IsSuccess => Errors.IsNullOrEmpty();
    }
}
