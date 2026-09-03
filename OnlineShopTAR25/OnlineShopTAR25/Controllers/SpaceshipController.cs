using Microsoft.AspNetCore.Mvc;

namespace OnlineShopTAR25.Controllers
{
    public class SpaceshipController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
