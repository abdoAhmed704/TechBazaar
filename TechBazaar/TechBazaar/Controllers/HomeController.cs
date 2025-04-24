using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index(string sTerm = "", int categoryId = 0, int  brandId = 0)
        {

            return View();
        }
    }
}
