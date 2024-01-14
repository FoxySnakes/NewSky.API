namespace NewSky.API.Models.Dto
{
    public class TebexSaleDto
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public string Status { get; set; }

        public DateTime Date { get; set; }

        public List<TebexSalePackagesDto> Packages { get; set; } = new List<TebexSalePackagesDto>();

        public TebexBuyerDto Buyer { get; set; }
    }

    public class TebexSalePackagesDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }
    }
}
