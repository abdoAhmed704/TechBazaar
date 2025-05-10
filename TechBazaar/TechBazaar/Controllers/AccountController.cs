using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;

namespace TechBazaar.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;

        public AccountController(UserManager<ApplicationUser> userManager,SignInManager<ApplicationUser> signInManager,IHttpContextAccessor httpContextAccessor)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
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
            if(await userManager.FindByEmailAsync(user.Email) != null)
            {
                ModelState.AddModelError("", "Email already exists.");
                return View(user);
            }
            // Check if the username is already taken
            if(await userManager.FindByNameAsync(user.UserName) != null)
            {
                ModelState.AddModelError("", "Username already exists.");
                return View(user);
            }
            // Create a new user
            var newUser = new ApplicationUser
            {
                UserName = user.UserName,
                Email = user.Email,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Street = user.Street,
                BuldingNo = user.BuldingNo,
                Floor = user.Floor,
                AppartmentNo = user.AppartmentNo,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode,
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
        [HttpGet]
        public async Task<IActionResult> UpdateAccount()
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId ?? "");

            if (user == null)
            {
                throw new Exception("User not found");
            }
            var model = new UpdateAccountModelView()
            {
                UserName = user.UserName,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Street = user.Street,
                BuldingNo = user.BuldingNo,
                Floor = user.Floor,
                AppartmentNo = user.AppartmentNo,
                City = user.City,
                Country = user.Country,
                PostalCode = user.PostalCode
            };
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateAccount(UpdateAccountModelView model)
        {
            var userId = httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await userManager.FindByIdAsync(userId ?? "");
            if (user == null)
            {
                throw new Exception("User not found");
            }
            if (!ModelState.IsValid) return View(model);
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            user.Street = model.Street;
            user.BuldingNo = model.BuldingNo;
            user.Floor = model.Floor;
            user.AppartmentNo = model.AppartmentNo;
            user.City = model.City;
            user.Country = model.Country;
            user.PostalCode = model.PostalCode;
            var result = await userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                return View(model);
            }

            return RedirectToAction("Index","Home");
        }
    }
}
