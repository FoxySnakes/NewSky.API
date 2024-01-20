namespace NewSky.API.Attributs
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class IsAdminPanelPermissionAttribute : Attribute
    {
        public bool IsAdmin { get; }

        public IsAdminPanelPermissionAttribute(bool isAdmin)
        {
            IsAdmin = isAdmin;
        }
    }

}
