namespace NewSky.API.Models.Dto
{
    public class TebexSalesPagedDto
    {
        public List<TebexSaleDto> Sales { get; set; } = new List<TebexSaleDto>();

        public int TotalCount { get; set; }
    }
}
