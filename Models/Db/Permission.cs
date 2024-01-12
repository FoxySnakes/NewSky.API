using System.ComponentModel;

namespace NewSky.API.Models.Db
{
    public class Permission : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsAdminPermission { get; set; }
    }

    public static class PermissionName
    {
        [Description("Accorde l'accès au panel admin du site")]
        public static readonly string AccessToAdminPanel = "admin-panel-access";
    }
}
