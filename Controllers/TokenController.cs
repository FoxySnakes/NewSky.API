using Microsoft.AspNetCore.Mvc;
using NewSky.API.Services;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : Controller
    {

        private readonly ITokenService _tokenService;
        private readonly ISecurityService _securityService;

        public TokenController(ITokenService tokenService, ISecurityService securityService)
        {
            _tokenService = tokenService;
            _securityService = securityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInvalidTokenAsync()
        {
            var result = await _tokenService.GetTokensAsync();

            return Ok(result);
        }

        [HttpPost("create/{token}")]
        public async Task<IActionResult> AddInvalidTokenAsync(string token)
        {

            var result = await _tokenService.CreateTokenInvalidAsync(token);

            return Ok(result);
        }

        [HttpPost("delete/{token}")]
        public async Task<IActionResult> DeleteInvalidTokenAsync(string token)
        {
            var result = await _tokenService.DeleteTokenInvalidAsync(token);

            return Ok(result);
        }

    }
}
