// Controllers/CheckoutController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TechBazaar.Core.Models;
using TechBazaar.Core.ViewModels;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public CheckoutController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public IActionResult Index(int cartId)
        {
            var cart = unitOfWork.Cart.GetCartById(cartId);
            if (cart == null) return NotFound();

            var user = cart.ApplicationUser;

            var viewModel = new CheckoutViewModel
            {
                CartId = cart.Id,
                UserFullName = $"{user.FirstName} {user.LastName}",
                Address = $"{user.Street}, Bldg {user.BuldingNo}, Floor {user.Floor}, Apt {user.AppartmentNo}, {user.City}, {user.Country}, {user.PostalCode}",
                TotalAmount = cart.TotalPrice(),
                PaymentMethods = unitOfWork.Checkout.GetActivePaymentMethods()
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult Process(CheckoutViewModel model)
        {
            var cart = unitOfWork.Cart.GetCartById(model.CartId);
            if (cart == null) return NotFound();

            var payment = new Payment
            {
                OrderId = cart.Id,
                PaymentMethodId = model.SelectedPaymentMethodId,
                Amount = model.TotalAmount
            };

            unitOfWork.Checkout.AddPayment(payment);
            unitOfWork.Checkout.UpdateCartStatus(cart, CartStatus.Paid);
            unitOfWork.SaveChanges();

            return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }
    }
}
