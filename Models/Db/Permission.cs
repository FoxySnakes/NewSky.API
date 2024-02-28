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

        [Description("Accès à la vue du dashboard sur le panel admin")]
        public const string AccessToDashboardOnAdminPanel = "access:admin-panel_dashboard";

        [Description("Accès à la vue des ventes sur le panel admin")]
        public const string AccessToSalesOnAdminPanel = "access:admin-panel_sales";

        [Description("Accès à la vue des utilisateurs sur le panel admin")]
        public const string AccessToUsersOnAdminPanel = "access:admin-panel_users";

        [Description("Accès à la vue des votes sur le panel admin")]
        public const string AccessToVotesOnAdminPanel = "access:admin-panel_votes";

        [Description("Accès à la vue des rôles sur le panel admin")]
        public const string AccessToRolesOnAdminPanel = "access:admin-panel_roles";

        [Description("Accès à la vue des paramètres généraux")]
        public const string AccessToGeneralSettingsOnAdminPanel = "access:admin-panel_general-settings";


        // Create

        [Description("Créer un Rôle")]
        public const string CreateRole = "create:role";


        // Update

        [Description("Modifier les rôles et nom d'utilisateur d'un utilisateur")]
        public const string UpdateUserInformations = "update:user_informations";

        [Description("Modifier la sanction de l'utilisateur")]
        public const string UpdateUserPunishment = "update:user_punishment";

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
