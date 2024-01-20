using NewSky.API.Models.Db;

namespace NewSky.API.Models.Dto
{
    public class RoleDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<PermissionDto> Permissions { get; set; } = new List<PermissionDto>();
    }
}
