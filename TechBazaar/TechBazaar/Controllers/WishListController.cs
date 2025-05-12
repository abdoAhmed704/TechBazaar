using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using TechBazaar.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using TechBazaar.Ef;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Web.Controllers
{
    [Authorize] 
    public class WishListController : Controller
    {
        private readonly EContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUnitOfWork unitOfWork;

        public WishListController(EContext context, UserManager<ApplicationUser> userManager,IUnitOfWork unitOfWork)
        {
            _context = context;
            _userManager = userManager;
            this.unitOfWork = unitOfWork;
        }

        
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var wishList = await _context.WishLists
                .Include(w => w.WishItems)
                .ThenInclude(wi => wi.Product)
                .ThenInclude(p => p.Images)
                .FirstOrDefaultAsync(w => w.UserId == userId && w.IsActive);

            if (wishList == null)
            {
                wishList = new WishList
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
                _context.WishLists.Add(wishList);
                await _context.SaveChangesAsync();
            }

            return View(wishList);
        }

        
        [HttpPost]
        public async Task<IActionResult> AddToWishList(int productId, int quantity = 1)
        {
            var userId = _userManager.GetUserId(User);
            var wishList = await _context.WishLists
                .FirstOrDefaultAsync(w => w.UserId == userId && w.IsActive);

            if (wishList == null)
            {
                wishList = new WishList
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                };
                _context.WishLists.Add(wishList);
                await _context.SaveChangesAsync();
            }

            var existingItem = await _context.WishItems
                .FirstOrDefaultAsync(wi => wi.WishId == wishList.Id && wi.ProductId == productId);

            if (existingItem == null)
            {
                var wishItem = new WishItem
                {
                    WishId = wishList.Id,
                    ProductId = productId,
                    Quantity = quantity
                };
                _context.WishItems.Add(wishItem);
            }
            else
            {
                existingItem.Quantity += quantity;
            }

            wishList.ModifiedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        
        [HttpPost]
        public async Task<IActionResult> RemoveFromWishList(int wishItemId)
        {
            var userId = _userManager.GetUserId(User);
            var wishItem = await _context.WishItems
                .FirstOrDefaultAsync(wi => wi.Id == wishItemId && wi.WishList.UserId == userId);

            if (wishItem != null)
            {
                _context.WishItems.Remove(wishItem);
                var wishList = await _context.WishLists.FindAsync(wishItem.WishId);
                if (wishList != null)
                {
                    wishList.ModifiedAt = DateTime.Now;
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> GetTotalItemInWishList()
        {
            int wishListItemCount = await unitOfWork.WishList.GetTotalItemInWishList();
            return Ok(wishListItemCount);
        }
    }
}