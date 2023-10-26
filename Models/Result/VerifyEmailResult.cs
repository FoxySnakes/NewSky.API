using Microsoft.AspNetCore.Identity;

namespace NewSky.API.Models.Result
{
    public class VerifyEmailResult
    {
        public bool IsSucceded { get; set; }
        public List<IdentityError> Errors { get; set; } = new List<IdentityError>();
    }
}
