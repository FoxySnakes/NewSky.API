using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.IdentityModel.Tokens;
using NewSky.API.Models.Db;
using NewSky.API.Models.Dto;
using NewSky.API.Models.Result;
using NewSky.API.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;

namespace NewSky.API.Services
{
    public class TebexService : ITebexService
    {
        private readonly HttpClient _headlessApi;
        private readonly HttpClient _pluginApi;
        private readonly string _webStoreIdentifier;
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IRepository<Package> _packageRepository;
        private readonly IRepository<UserPackage> _userPackageRepository;

        public TebexService(IMemoryCache memoryCache, HttpClient headlessApi, HttpClient pluginApi, IMapper mapper, IRepository<Package> packageRepository, IRepository<UserPackage> userPackageRepository)
        {
            _memoryCache = memoryCache;
            _mapper = mapper;
            _packageRepository = packageRepository;
            _userPackageRepository = userPackageRepository;



            _headlessApi = headlessApi;
            _headlessApi.BaseAddress = new Uri("https://headless.tebex.io");
            _webStoreIdentifier = "q9p1-a1dc8b36df104edf3ec62d84d3851a78ef5dcf51";

            _pluginApi = pluginApi;
            _pluginApi.BaseAddress = new Uri("https://plugin.tebex.io");
            _pluginApi.DefaultRequestHeaders.Add("X-Tebex-Secret", "3421979d0b799fe1733ce64ad591b28d3bc86b77");

#if DEBUG
            _webStoreIdentifier = "rrx1-9e82a64dc20135a1c070e4602c5c8176c0e945a0";
            _pluginApi.DefaultRequestHeaders.Remove("X-Tebex-Secret");
            _pluginApi.DefaultRequestHeaders.Add("X-Tebex-Secret", "8d6f1ee36dc2ee4ff3221c2a14b32221d8be2332");
#endif
        }

        public async Task<List<TebexCategoryDto>> GetCategoriesAsync(bool withPackages)
        {
            var response = await _headlessApi.GetAsync($"/api/accounts/{_webStoreIdentifier}/categories?includePackages={(withPackages ? 1 : 0)}");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(responseContent)["data"];
            var categories = new List<TebexCategoryDto>();
            var packages = new List<TebexPackageDto>();

            foreach (var category in data)
            {
                var tebexCategory = new TebexCategoryDto()
                {
                    Id = int.Parse(category["id"].ToString()),
                    Name = category["name"].ToString(),
                    Description = category["description"].ToString(),
                    Order = int.Parse(category["order"].ToString()),
                };
                foreach (var package in category["packages"])
                {
                    var tebexPackage = new TebexPackageDto()
                    {
                        Id = int.Parse(package["id"].ToString()),
                        Name = package["name"].ToString(),
                        ImageUrl = package["image"].ToString(),
                        PriceHt = decimal.Parse(package["base_price"].ToString()),
                        PriceTtc = decimal.Parse(package["total_price"].ToString()),
                        CreationDate = package["created_at"].ToString().IsNullOrEmpty() || package["created_at"] == null ? null : DateTime.Parse(package["created_at"].ToString()),
                        ExpirationDate = package["expiration_date"].ToString().IsNullOrEmpty() || package["expiration_date"] == null ? null : DateTime.Parse(package["expiration_date"].ToString())
                    };
                    tebexCategory.Packages.Add(tebexPackage);
                    packages.Add(tebexPackage);
                }
                categories.Add(tebexCategory);
            }

            await VerifyPackageInformationAsync(packages);

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
            };
            _memoryCache.Set("categories", categories, cacheEntryOptions);

            return categories;
        }

        public async Task<TebexSalesPagedDto> GetSalesAsync(int pageNumber)
        {
            var response = await _pluginApi.GetAsync($"payments?paged={pageNumber}");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(responseContent)["data"];
            var sales = new List<TebexSaleDto>();
            foreach (var sale in data)
            {
                var tebexSale = new TebexSaleDto()
                {
                    Id = int.Parse(sale["id"].ToString()),
                    Price = decimal.Parse(sale["amount"].ToString(), CultureInfo.InvariantCulture),
                    Date = DateTime.Parse(sale["date"].ToString()),
                    Status = sale["status"].ToString(),
                    Buyer = new TebexBuyerDto()
                    {
                        Id = int.Parse(sale["player"]["id"].ToString()),
                        UserName = sale["player"]["name"].ToString(),
                        UUID = sale["player"]["uuid"].ToString()
                    }
                };

                foreach (var package in sale["packages"])
                {
                    tebexSale.Packages.Add(new TebexSalePackagesDto()
                    {
                        Id = int.Parse(package["id"].ToString()),
                        Name = package["name"].ToString(),
                        Quantity = int.Parse(package["quantity"].ToString()),

                    });
                }
                sales.Add(tebexSale);
            }

            var tebexSalesPagedDto = new TebexSalesPagedDto()
            {
                Sales = sales,
                TotalCount = int.Parse(JObject.Parse(responseContent)["total"].ToString())
            };

            var cacheEntryOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30)
            };
            _memoryCache.Set($"sales?paged={pageNumber}", tebexSalesPagedDto, cacheEntryOptions);

            return tebexSalesPagedDto;
        }

        public async Task<PackageManageResult> ManagePackageOnCartAsync(int userId, long packageTebexId, int quantity)
        {
            var result = new PackageManageResult();
            var package = await _packageRepository.Query().FirstOrDefaultAsync(x => x.TebexId == packageTebexId);
            if (package != null)
            {
                var existingPackage = await _userPackageRepository.Query().FirstOrDefaultAsync(x => x.UserId == userId && x.PackageId == package.Id);
                if (quantity == 0)
                {
                    if (existingPackage != null)
                    {
                        var deleteResult = await _userPackageRepository.DeleteAsync(existingPackage.Id);
                        if (!deleteResult.IsSuccess)
                        {
                            result.Result.Errors.Add(deleteResult.Errors.Select(e => e.Message).First());
                        }
                    }
                    else
                    {
                        result.Result.Errors.Add("This Package doesn't exist in the cart of the User");

                    }
                }
                else
                {
                    if (existingPackage == null)
                    {
                        var createResult = await _userPackageRepository.CreateAsync(new UserPackage { UserId = userId, PackageId = package.Id, Quantity = quantity });
                        if (!createResult.IsSuccess)
                        {
                            result.Result.Errors.Add(createResult.Errors.Select(e => e.Message).First());
                        }
                    }
                    else
                    {
                        existingPackage.Quantity = quantity;
                        var updateResult = await _userPackageRepository.UpdateAsync(existingPackage.Id);
                        if (!updateResult.IsSuccess)
                        {
                            result.Result.Errors.Add(updateResult.Errors.Select(e => e.Message).First());
                        }
                    }
                }
            }
            else
            {
                result.Result.Errors.Add("No package exist with this Id");
            }
            var userPackages = await _userPackageRepository.Query().Where(x => x.UserId == userId).ToListAsync();
            foreach (var userPackage in userPackages)
            {
                result.Packages.Add(new PackageCartDto()
                {
                    TebexPackage = _mapper.Map<TebexPackageDto>(userPackage.Package),
                    Quantity = userPackage.Quantity,
                });
            }
            return result;
        }

        public async Task<PackageManageResult> ClearUserCartAsync(int userId)
        {
            var userPackages = await _userPackageRepository.Query().Where(x => x.UserId == userId).Include(x => x.Package).ToListAsync();
            var userPackagesFailed = new List<UserPackage>();
            var result = new PackageManageResult();
            foreach (var userPackage in userPackages)
            {
                var deleteResult = await _userPackageRepository.DeleteAsync(userPackage.Id);
                if(!deleteResult.IsSuccess)
                {
                    result.Result.Errors.Add($"Impossible de supprimer le package '{userPackage.Package.Name}'");
                    userPackagesFailed.Add(userPackage);
                }
            }

            foreach (var userPackageFailed in userPackagesFailed)
            {
                result.Packages.Add(new PackageCartDto()
                {
                    TebexPackage = _mapper.Map<TebexPackageDto>(userPackageFailed.Package),
                    Quantity = userPackageFailed.Quantity,
                });
            }

            return result;
        }

        public async Task<string> GetLinkTebexCartAsync(int userId)
        {
            var userPackages = await _userPackageRepository.Query().Where(x => x.UserId == userId).Include(x => x.Package).ToListAsync();
            var linkTebexCart = "";
            if(userPackages.Count > 0)
            {
                var createBasketData = new { username = "FoxySnake" };
                var createBasketResponse = await _headlessApi.PostAsJsonAsync($"/api/accounts/{_webStoreIdentifier}/baskets", createBasketData);
                if (createBasketResponse.IsSuccessStatusCode)
                {
                    string createBasketResponseContent = await createBasketResponse.Content.ReadAsStringAsync();
                    var createData = JObject.Parse(createBasketResponseContent)["data"];
                    var basketIndent = createData["ident"].ToString();
                    foreach(var userPackage  in userPackages)
                    {
                        var addPackageData = new { package_id = userPackage.Package.TebexId, quantity = userPackage.Quantity };
                        var addPackageResponse = await _headlessApi.PostAsJsonAsync($"/api/baskets/{basketIndent}/packages", addPackageData);
                        if(addPackageResponse.IsSuccessStatusCode)
                        {
                            string addPackageResponseContent = await addPackageResponse.Content.ReadAsStringAsync();
                            var addData = JObject.Parse(addPackageResponseContent)["data"];
                            linkTebexCart = addData["links"]["checkout"].ToString();
                        }
                    }
                }
            }

            return linkTebexCart;
        }

        private async Task VerifyPackageInformationAsync(List<TebexPackageDto> tebexPackages)
        {
            var packages = _mapper.Map<List<Package>>(tebexPackages);
            var storedPackages = await _packageRepository.Query().ToListAsync();

            foreach (var package in packages)
            {
                var currentPackage = storedPackages.Find(x => x.TebexId == package.TebexId);

                if (currentPackage == null)
                {
                    var result = await _packageRepository.CreateAsync(package);
                }
                else if (!currentPackage.Equals(package))
                {
                    _mapper.Map(package, currentPackage);
                    var result = await _packageRepository.UpdateAsync(currentPackage.Id);
                }
                storedPackages.Remove(currentPackage);
            }
            foreach (var storedPackage in storedPackages)
            {
                await _packageRepository.DeleteAsync(storedPackage.Id);
            }
        }
    }
}
