using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using NewSky.API.Models.Dto;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TebexController : Controller
    {
        private readonly ITebexService _tebexService;
        private readonly IMemoryCache _memoryCache;
        private readonly IUserService _userService;

        public TebexController(ITebexService tebexService, IMemoryCache memoryCache, IUserService userService)
        {
            _tebexService = tebexService;
            _memoryCache = memoryCache;
            _userService = userService;
        }

        [HttpGet("listing")]
        public async Task<IActionResult> GetCategoriesAsync([FromQuery] bool withPackages)
        {

            if (_memoryCache.TryGetValue("categories", out List<TebexCategoryDto> tebexCategories))
            {
                return Ok(tebexCategories);
            }
            var categories = await _tebexService.GetCategoriesAsync(withPackages);
            return Ok(categories);
        }

        [HttpGet("last-sales")]
        public async Task<IActionResult> GetLastSalesAsync()
        {
            if (_memoryCache.TryGetValue("sales", out List<TebexSaleDto> tebexSales))
            {
                return Ok(tebexSales.Where(x => x.Status == "Complete").Take(5).ToList());
            }
            var sales = await _tebexService.GetSalesAsync(1);
            var lastBuyers = sales.Where(x => x.Status == "Complete").Take(5).ToList();
            return Ok(lastBuyers);
        }

        [HttpPost("manage-package")]
        public async Task<IActionResult> ManagePackageOnCart([FromBody] ManagePackageDto addPackageDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _tebexService.ManagePackageOnCartAsync(user.Id, addPackageDto.PackageTebexId, addPackageDto.Quantity);
            return Ok(result);
        }
    }
}
