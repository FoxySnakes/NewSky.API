using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
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
        private readonly ILogger<TebexController> _logger;

        public TebexController(ITebexService tebexService, IMemoryCache memoryCache, IUserService userService, ILogger<TebexController> logger)
        {
            _tebexService = tebexService;
            _memoryCache = memoryCache;
            _userService = userService;
            _logger = logger;
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
            if (_memoryCache.TryGetValue("sales?paged=1", out TebexSalesPagedDto tebexSales))
            {
                return Ok(tebexSales.Sales.Where(x => x.Status == "Complete").Take(5).ToList());
            }
            tebexSales = await _tebexService.GetSalesAsync(1);
            var lastBuyers = tebexSales.Sales.Where(x => x.Status == "Complete").Take(5).ToList();
            return Ok(lastBuyers);
        }

        [HttpPost("manage-package")]
        public async Task<IActionResult> ManagePackageOnCart([FromBody] ManagePackageDto addPackageDto)
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _tebexService.ManagePackageOnCartAsync(user.Id, addPackageDto.PackageTebexId, addPackageDto.Quantity);

            if (!result.Result.Success)
                _logger.LogError("Failed adding x{Times} times package with Id {PackageId} for user {Username}", addPackageDto.Quantity, addPackageDto.PackageTebexId,user.UserName);

            return Ok(result);
        }

        [HttpDelete("clear-cart")]
        public async Task<IActionResult> ClearUserCart()
        {
            var user = await _userService.GetCurrentUserAsync();
            var result = await _tebexService.ClearUserCartAsync(user.Id);

            if (!result.Result.Success)
                _logger.LogError("Failed clearing cart of user {Username}", user.UserName);

            return Ok(result);
        }

        [HttpGet("cart-link")]
        public async Task<IActionResult> GetLinkUserCart()
        {
            var user = await _userService.GetCurrentUserAsync();
            var linkUser = await _tebexService.GetLinkTebexCartAsync(user.Id);

            if (linkUser.IsNullOrEmpty())
                _logger.LogWarning("Failed creating Tebex Cart for user {Username}", user.UserName);

            return Ok(new { linkUserCart = linkUser });
        }

        [HttpGet("sales")]
        public async Task<IActionResult> GetSales([FromQuery] int paged = 1, [FromQuery] bool refresh = false)
        {
            if (!refresh && _memoryCache.TryGetValue($"sales?paged={paged}", out TebexSalesPagedDto tebexSalesPagedDto))
            {
                return Ok(tebexSalesPagedDto);
            }
            tebexSalesPagedDto = await _tebexService.GetSalesAsync(paged);
            return Ok(tebexSalesPagedDto);
        }
    }
}
