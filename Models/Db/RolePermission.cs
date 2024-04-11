namespace NewSky.API.Models.Db
{
    public class RolePermission : EntityBaseWithId
    {
        public int RoleId { get; set; }

        public Role Role { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }

        public bool IsEditable { get; set; } = true;

        public bool HasPermission { get; set; } = true;
    }
}
