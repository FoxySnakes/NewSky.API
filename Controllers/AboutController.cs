using Microsoft.AspNetCore.Mvc;

namespace NewSky.API.Controllers
{
    [Route("[controller]")]
    public class AboutController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
