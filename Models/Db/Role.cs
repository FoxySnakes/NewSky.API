namespace NewSky.API.Models.Db
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public List<RolePermission> Permissions { get; set; } = new List<RolePermission>();

        public int Level { get; set; }
    }
}
