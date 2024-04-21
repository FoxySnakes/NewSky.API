using Microsoft.AspNetCore.Mvc;


namespace NewSky.API.Controllers
{
    [Route("[controller]")]
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
    }
}
