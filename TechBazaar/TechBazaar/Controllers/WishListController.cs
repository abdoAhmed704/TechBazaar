using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;
using TechBazaar.Ef;

namespace TechBazaar.Web.Controllers
{
    public class WishListController : Controller
    {
        private readonly EContext _context;
        private readonly IUnitOfWork unitOfWork;

        public WishListController(EContext context)
        {
            _context = context;
        }

        // GET: WishList
        public async Task<IActionResult> Index()
        {
            var wishLists = await _context.WishLists
                .Include(w => w.ApplicationUser)
                .Include(w => w.WishItems)
                .ThenInclude(wi => wi.Product)
                .ToListAsync();
            return View(wishLists);
        }

        // GET: WishList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var wishList = await _context.WishLists
                .Include(w => w.ApplicationUser)
                .Include(w => w.WishItems)
                .ThenInclude(wi => wi.Product)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wishList == null) return NotFound();

            return View(wishList);
        }

        // GET: WishList/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: WishList/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(WishList wishList)
        {
            if (ModelState.IsValid)
            {
                wishList.ModifiedAt = DateTime.Now;
                _context.Add(wishList);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(wishList);
        }

        // GET: WishList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList == null) return NotFound();

            return View(wishList);
        }

        // POST: WishList/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, WishList wishList)
        {
            if (id != wishList.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    wishList.ModifiedAt = DateTime.Now;
                    _context.Update(wishList);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.WishLists.Any(e => e.Id == id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(wishList);
        }

        // GET: WishList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var wishList = await _context.WishLists
                .FirstOrDefaultAsync(m => m.Id == id);

            if (wishList == null) return NotFound();

            return View(wishList);
        }

        // POST: WishList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var wishList = await _context.WishLists.FindAsync(id);
            if (wishList != null)
            {
                _context.WishLists.Remove(wishList);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
