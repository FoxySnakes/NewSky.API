using NewSky.API.Models.Dto;

namespace NewSky.API.Services.Interface
{
    public interface ITebexService
    {
        Task<TebexListing> GetListingAsync();
    }
}
