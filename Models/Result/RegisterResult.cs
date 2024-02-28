using Microsoft.IdentityModel.Tokens;
using NewSky.API.Models.Dto;

namespace NewSky.API.Models.Result
{
    public class RegisterResult
    {
        public UserDto User { get; set; }
        public string Token { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public bool IsSuccess => Errors.IsNullOrEmpty();
    }
}
