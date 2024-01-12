using Microsoft.AspNetCore.Mvc;
using NewSky.API.Services.Interface;

namespace NewSky.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TebexController : Controller
    {
        private readonly ITebexService _tebexService;
        public TebexController(ITebexService tebexService)
        {
            _tebexService = tebexService;
        }
    }
}
