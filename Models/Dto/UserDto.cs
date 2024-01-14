using Microsoft.AspNetCore.Identity;
using NewSky.API.Models.Db;
using System.Data;

namespace NewSky.API.Models.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string UUID { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public List<string> Permissions { get; set; } = new List<string>();

        public List<PackageCartDto> Packages { get; set; } = new List<PackageCartDto>();
    }
}
