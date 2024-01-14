namespace NewSky.API.Models.Dto
{
    public class TebexPackageDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? ImageUrl { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreationDate { get; set; }
    }
}
