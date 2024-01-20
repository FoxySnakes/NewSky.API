namespace NewSky.API.Models.Db
{
    public class UserPermission : EntityBase
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int PermissionId { get; set; }

        public Permission Permission { get; set; }

        public bool HasPermission { get; set; } = true;

    }
}
