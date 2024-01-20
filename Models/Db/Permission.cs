using NewSky.API.Attributs;
using System.ComponentModel;

namespace NewSky.API.Models.Db
{
    public class Permission : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }
    }




    // Lors des changements içi, pensez à refaire une migration ainsi qu'à modifier
    // l'objet AdminPanelPermissionDto si la permission modifier concerne le panel admin
    public static class PermissionName 
    {
        // Access

        [Description("Accès au panel admin du site")]
        public const string AccessToAdminPanel = "access:admin-panel";

        [Description("Accès à la vue des ventes sur le panel admin")]
        public const string AccessToSalesOnAdminPanel = "access:admin-panel_sales";

        [Description("Accès à la vue des utilisateurs sur le panel admin")]
        public const string AccessToUsersOnAdminPanel = "access:admin-panel_users";

        [Description("Accès à la vue des votes sur le panel admin")]
        public const string AccessToVotesOnAdminPanel = "access:admin-panel_votes";

        [Description("Accès à la vue des paramètres généraux")]
        public const string AccessToGeneralSettingsOnAdminPanel = "access:admin-panel_general-settings";


        // Create

        [Description("Créer un Rôle")]
        public const string CreateRole = "create:role";


        // Update

        [Description("Modifier les permissions d'un utilisateur")]
        public const string UpdateUserPermissions = "update:user_permissions";

        [Description("Modifier le pseudo d'un utilisateur")]
        public const string UpdateUserUserName = "update:user_username";

        [Description("Gérer l'état d'un compte utilisateur")]
        public const string UpdateUserStatus = "update:user_status";

        [Description("Gérer le panier de l'utilisateur authentifié")]
        public const string ManageUserCart = "update:user_cart";

        [Description("Modifier les paramètres généraux du site")]
        public const string UpdateGeneralSettings = "update:general-settings";

        [Description("Modifier le rôle d'un utilisateur")]
        public const string UpdateUserRole = "update:user_role";

        [Description("Modifier un rôle")]
        public const string UpdateRole = "update:role";


        // Delete

        [Description("Supprimer un rôle")]
        public const string DeleteRole = "delete:role";
    }
}
