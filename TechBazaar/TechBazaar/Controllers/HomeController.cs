using Microsoft.AspNetCore.Mvc;

namespace TechBazaar.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
