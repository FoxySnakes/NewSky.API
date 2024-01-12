namespace NewSky.API.Models.Dto
{
    public class PaginationFilterParamsDto
    {
        public string Search { get; set; }

        public int PageSize { get; set; }

        public int PageNumber { get; set; }

        public string Filter { get; set; }

        public string Direction { get; set; }
    }
}
