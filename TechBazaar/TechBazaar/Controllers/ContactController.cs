// Controllers/ContactController.cs
using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.ViewModels;

namespace TechBazaar.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View(new ContactViewModel());
        }

        [HttpPost]
        public IActionResult Send(ContactViewModel model)
        {
            if (!ModelState.IsValid)
                return View("Index", model);

            TempData["MessageSent"] = "Your message has been sent successfully. We will get back to you shortly.";

            return RedirectToAction("Index");
        }
    }
}
