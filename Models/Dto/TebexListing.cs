namespace NewSky.API.Models.Dto
{
    public class TebexListing
    {
        public IList<TebexCategory> Categories { get; set; } = new List<TebexCategory>();
    }

    public class TebexCategory
    {
        public int Id { get; set; }
        
        public string Name { get; set; }

        public string? Description { get; set; }

        public TebexCategory CategoryParent { get; set; }

        public IList<TebexPackage> Packages { get; set; } = new List<TebexPackage>();

        public int Order { get; set; }
    }

    public class TebexPackage
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string? ImageUrl { get; set; }

        public float BasePrice { get; set; }

        public float SalesPrice { get; set; }

        public float TotalPrice { get; set; }

        public string Currency { get; set; }

        public int Discount { get; set; }

        public bool GiftingEnable { get; set; }

        public DateTime? ExpirationDate { get; set; }

        public DateTime? CreationDate { get; set; }

    }
}
