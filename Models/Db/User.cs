namespace NewSky.API.Models.Db
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string UUID { get; set; }

        public string Email { get; set; }

        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        public int RoleId { get; set; }

        public string PasswordHash { get; set; }

        public List<UserPermission> Permissions { get; set; } = new List<UserPermission>();

        public TimeSpan LockoutEnd { get; set; }

        public bool IsLocked
        {
            get
            {
                return LockoutEnd > TimeSpan.Zero;
            }
            private set { }
        }

        public TimeSpan BanishmentEnd { get; set; }

        public bool IsBanned
        {
            get
            {
                return BanishmentEnd > TimeSpan.Zero;
            }
            private set { }
        }

        public int AccessFailedCount { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
