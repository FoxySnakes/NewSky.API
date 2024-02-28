namespace NewSky.API.Models.Dto
{
    public class AdminPanelPermissionDto
    {
        public bool AccessToSalesOnAdminPanel { get; set; } = false;

        public bool AccessToUsersOnAdminPanel { get; set; } = false;

        public bool AccessToVotesOnAdminPanel { get; set; } = false;

        public bool AccessToGeneralSettingsOnAdminPanel { get; set; } = false;


        // Create

        public bool CreateRole { get; set; } = false;


        // Update

        public bool UpdateUserPermissions { get; set; } = false;

        public bool UpdateUserUserName { get; set; } = false;

        public bool UpdateUserStatus { get; set; } = false;

        public bool ManageUserCart { get; set; } = false;

        public bool UpdateGeneralSettings { get; set; } = false;

        public bool UpdateUserRole { get; set; } = false;

        public bool UpdateRole { get; set; } = false;


        // Delete

        public bool DeleteRole { get; set; } = false;
    }
}
