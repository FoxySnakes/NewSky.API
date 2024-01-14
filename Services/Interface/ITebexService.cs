using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;

namespace NewSky.API.Services.Interface
{
    public interface ITebexService
    {
        Task<List<TebexCategoryDto>> GetCategoriesAsync(bool withPackages);

        Task<List<TebexSaleDto>> GetSalesAsync(int pageNumber);

        Task<PackageManageResult> ManagePackageOnCartAsync(int userId, long packageTebexId, int quantity);

    }
}
