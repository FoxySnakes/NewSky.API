namespace NewSky.API.Models.Dto
{
    public class UserDto
    {
        public string UserName { get; set; }

        public string UUID { get; set; }

        public string Email { get; set; }

        public List<string> Roles { get; set; } = new List<string>();

        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();

        public List<PackageCartDto> Packages { get; set; } = new List<PackageCartDto>();

        public DateTime? BanishmentEnd { get; set; } = null;

        public DateTime? LockoutEnd { get; set; } = null;
    }
}
