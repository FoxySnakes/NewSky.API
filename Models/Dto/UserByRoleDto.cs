using NewSky.API.Models.Db;

namespace NewSky.API.Models.Dto
{
    public class UserByRoleDto
    {
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public List<UserDto> Users { get; set; }
    }
}
