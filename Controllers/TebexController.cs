using Microsoft.AspNetCore.Mvc;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TebexController : Controller
    {
        private readonly HttpClient _httpClient;
        public TebexController(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.DefaultRequestHeaders.Add("X-Tebex-Secret", "3421979d0b799fe1733ce64ad591b28d3bc86b77");

        }

        [HttpGet("listing")]
        public async Task<IActionResult> GetListingAsync()
        {
            HttpResponseMessage response = await _httpClient.GetAsync("https://plugin.tebex.io/listing");
            response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadFromJsonAsync<Object>();
            return Ok();
        }
    }
}
