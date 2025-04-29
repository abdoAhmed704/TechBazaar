using Microsoft.AspNetCore.Mvc;
using System.Numerics;
using System.Threading.Tasks;
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

        public async Task<IActionResult> GetUserCart()
        {
            var cart = await unitOfWork.Cart.GetUserCart();
            return View(cart);
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var result = await unitOfWork.Cart.AddToCart(productId, quantity);
            if(result == false)
            {
                return BadRequest("Product not available or invalid quantity.");
            }
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
            var cartItemCount = await unitOfWork.Cart.GetTotalItemInCart();
            return Ok(cartItemCount);
        }
    }
}
