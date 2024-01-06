namespace NewSky.API.Models.Dto
{
    public class TebexListing
    {
        public IList<TebexCategory> Categories { get; set; }
    }

    public class TebexCategory
    {
        public int Id { get; set; }
        
        public int Order { get; set; }

        public TebexCategory SubCategory { get; set; }

        public IList<TebexPackage> Packages { get; set; }
    }

    public class TebexPackage
    {
        public int Id { get; set; }

        public int Order { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

    }
}
