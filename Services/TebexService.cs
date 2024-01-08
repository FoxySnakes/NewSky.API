using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.IdentityModel.Tokens;
using NewSky.API.Models.Dto;
using NewSky.API.Services.Interface;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace NewSky.API.Services
{
    public class TebexService : ITebexService
    {
        private readonly HttpClient _httpClient;
        private readonly string _webStoreIdentifier;
        public TebexService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://headless.tebex.io");
            _webStoreIdentifier = "rrx1-9e82a64dc20135a1c070e4602c5c8176c0e945a0";
        }
        public async Task<TebexListing> GetListingAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync($"/api/accounts/{_webStoreIdentifier}/categories?includePackages=1");
            response.EnsureSuccessStatusCode();
            string responseContent = await response.Content.ReadAsStringAsync();
            var data = JObject.Parse(responseContent)["data"];
            var listing = new TebexListing();

            foreach(var category in data)
            {
                var tebexCategory = new TebexCategory()
                {
                    Id = int.Parse(category["id"].ToString()),
                    Name = category["name"].ToString(),
                    CategoryParent = JsonConvert.DeserializeObject<TebexCategory>(category["parent"].ToString()),
                    Description = category["description"].ToString(),
                    Order = int.Parse(category["order"].ToString()),
                };
                foreach(var package in category["packages"])
                {
                    var tebexPackage = new TebexPackage()
                    {
                        Id = int.Parse(package["id"].ToString()),
                        Name = package["name"].ToString(),
                        Description = package["description"].ToString(),
                        ImageUrl = package["image"].ToString(),
                        BasePrice = float.Parse(package["base_price"].ToString()),
                        SalesPrice = float.Parse(package["sales_tax"].ToString()),
                        TotalPrice = float.Parse(package["total_price"].ToString()),
                        Currency = package["currency"].ToString(),
                        Discount = int.Parse(package["discount"].ToString()),
                        GiftingEnable = bool.Parse(package["disable_gifting"].ToString()),
                        CreationDate = package["created_at"].ToString().IsNullOrEmpty() ? null : DateTime.Parse(package["created_at"].ToString()),
                        ExpirationDate = package["expiration_date"].ToString().IsNullOrEmpty() ? DateTime.MaxValue : DateTime.Parse(package["expiration_date"].ToString())
                    };
                    tebexCategory.Packages.Add(tebexPackage);
                }
                listing.Categories.Add(tebexCategory);
            }
            return listing;
        }
    }
}
