namespace NewSky.API.Models.Dto
{
    public class TebexPackageDto
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal PriceHt { get; set; }

        public decimal PriceTtc { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
