using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef.Repository
{
    public class WishListRepository<T> : BaseRepository<T>, IWishListRepository<T> where T : WishList
    {
        private readonly EContext eContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public WishListRepository(EContext eContext,IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager) : base(eContext)
        {
            this.eContext = eContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }

        public async Task<IEnumerable<int>> GetWishListProductIds()
        {
            var userId = GetUserId();
            var wishListProductIds = await eContext.WishItems
                       .Where(wi => wi.WishList.UserId == userId)
                       .Select(wi => wi.ProductId)
                       .ToListAsync();
            return wishListProductIds;
        }

        public async Task<int> GetTotalItemInWishList()
        {
            var userId = GetUserId();
            var wishList = await eContext.WishLists.Include(c => c.WishItems).FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
            var wishListCount = wishList?.WishItems.Count();
            return wishListCount ?? 0;
        }
        private string GetUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var userId = userManager.GetUserId(user);

            if (userId == null)
            {
                return "";
            }
            return userId;
        }
    }
}
