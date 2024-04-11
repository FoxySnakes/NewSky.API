namespace NewSky.API.Models.Db
{
    public class UserPackage : EntityBaseWithId
    {
        public int UserId { get; set; }

        public User User { get; set; }

        public int PackageId { get; set; }

        public Package Package { get; set; }

        public int Quantity { get; set; }
    }
}
