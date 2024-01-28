using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface ITebexService
    {
        Task<List<TebexCategoryDto>> GetCategoriesAsync(bool withPackages);

        Task<TebexSalesPagedDto> GetSalesAsync(int pageNumber);

        Task<PackageManageResult> ManagePackageOnCartAsync(int userId, long packageTebexId, int quantity);

        Task<PackageManageResult> ClearUserCartAsync(int userId);

        Task<string> GetLinkTebexCartAsync(int userId);
    }
}
