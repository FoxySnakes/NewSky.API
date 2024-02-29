namespace NewSky.API.Models.Db
{
    public class User : EntityBase
    {
        public string UserName { get; set; }

        public string UUID { get; set; }

        public string Email { get; set; }

        public List<UserPackage> Packages { get; set; } = new List<UserPackage>();

        public List<UserRole> Roles { get; set; } = new List<UserRole>();

        public string PasswordHash { get; set; }

        public DateTime LockoutEnd { get; set; }

        public bool IsLocked
        {
            get
            {
                return LockoutEnd > DateTime.Now;
            }
            private set { }
        }

        public DateTime BanishmentEnd { get; set; }

        public bool IsBanned
        {
            get
            {
                return BanishmentEnd > DateTime.Now;
            }
            private set { }
        }

        public int AccessFailedCount { get; set; }

        public DateTime DateCreated { get; set; }
    }
}
