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
        public async Task<IActionResult> AddToCart(int productId, int quantity,int redirect = 0)
        {
            var result = await unitOfWork.Cart.AddToCart(productId, quantity);
            if(result == false)
            {
                return BadRequest("Product not available or invalid quantity.");
            }
            if(redirect == 1)
            {
                return RedirectToAction("GetUserCart");
            }
            return NoContent();
        }

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
