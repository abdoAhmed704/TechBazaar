// Controllers/CheckoutController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TechBazaar.Core.Models;
using TechBazaar.Core.ViewModels;
using TechBazaar.Core.Enums;

namespace TechBazaar.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public CheckoutController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null) return RedirectToAction("Login", "Account");

            var viewModel = new CheckoutViewModel
            {
                FullName = $"{user.FirstName} {user.LastName}",
                Email = user.Email,
                Street = user.Street,
                BuildingNo = user.BuldingNo,
                Floor = user.Floor,
                AppartmentNo = user.AppartmentNo,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode,
                OrderTotal = user.Carts
                    .FirstOrDefault(c => c.Status == CartStatus.Pending)?.TotalPrice() ?? 0
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult ConfirmOrder(CheckoutViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }


            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            ViewBag.Message = "Thank you for your order! Your purchase has been successfully confirmed.";
            return View();
        }
    }
}
