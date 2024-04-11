namespace NewSky.API.Models.Db
{
    public class Package : EntityBaseWithId
    {
        public long TebexId { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal PriceHt { get; set; }

        public decimal PriceTtc { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
