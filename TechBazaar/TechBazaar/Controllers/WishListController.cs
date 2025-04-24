using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Controllers
{
    public class WishListController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public WishListController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
