using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Controllers
{
    public class CartController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CartController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
