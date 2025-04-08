using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;

namespace TechBazaar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserRegisterModelView user)
        {
            // Check if the model is valid
            if (!ModelState.IsValid) return View(user);
            if(userManager.FindByEmailAsync(user.Email) != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(user);
            }
            // Check if the username is already taken
            if(userManager.FindByNameAsync(user.UserName) != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(user);
            }
            // Create a new user
            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
            };
            var result = await userManager.CreateAsync(newUser, user.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                     ModelState.AddModelError("", error.Description);
                }
                return View(user);
            }
            
            // Add user to default role
            await userManager.AddToRoleAsync(newUser, "User");

            return RedirectToAction("Login", "Account");
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UserLogInModelView user)
        {
            if (!ModelState.IsValid) return View(user);

            var currentUser = await userManager.FindByEmailAsync(user.Email);

            if(currentUser is null ){
                ModelState.AddModelError("", "Invalid email or password.");
                return View(user);
            }
            var result = await signInManager.PasswordSignInAsync(currentUser, user.Password,user.RememberMe,false);
            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Invalid email or password.");
                return View(user);
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
