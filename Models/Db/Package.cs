namespace NewSky.API.Models.Db
{
    public class Package : EntityBase
    {
        public long TebexId { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
