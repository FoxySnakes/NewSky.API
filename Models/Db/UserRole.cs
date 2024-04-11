namespace NewSky.API.Models.Db
{
    public class UserRole : EntityBaseWithId
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int RoleId { get; set; }

        public Role Role { get; set; }
    }
}
