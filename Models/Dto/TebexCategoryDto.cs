namespace NewSky.API.Models.Dto
{
    public class TebexCategoryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string? Description { get; set; }

        public int Order { get; set; }

        public List<TebexPackageDto> Packages { get; set; } = new List<TebexPackageDto>();
    }
}
