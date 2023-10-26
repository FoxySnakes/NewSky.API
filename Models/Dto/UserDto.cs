using Microsoft.AspNetCore.Identity;
using System.Data;

namespace NewSky.API.Models.Dto
{
    public class UserDto
    {

        public string UserName { get; set; }
        public string UUID { get; set; }
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string Role { get; set; }
    }
}
