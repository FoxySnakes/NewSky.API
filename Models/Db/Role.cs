using System.ComponentModel;

namespace NewSky.API.Models.Db
{
    public class Role : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public List<RolePermission> Permissions { get; set; } = new List<RolePermission>();

        public bool IsDefault { get; set; } = false;
    }

    public static class DefaultRole
    {
        [Description("Utilisateur authentifié")]
        public const string Player = "Joueur";

        [Description("Responsable du serveur")]
        public const string Owner = "Fondateur";

        [Description("Développeur du site")]
        public const string WebSiteDeveloper = "Développeur du site";
    }
}
