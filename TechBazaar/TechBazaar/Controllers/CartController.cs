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

        public IActionResult GetUserCart()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            await unitOfWork.Cart.AddToCart(productId, quantity);
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            await unitOfWork.Cart.RemoveFromCart(productId);
            return RedirectToAction("GetUserCart");
        }

        public async Task<IActionResult> GetTotalItemInCart()
        {

        }
    }
}
