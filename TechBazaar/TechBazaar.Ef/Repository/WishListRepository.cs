using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private string GetUserId()
        {
            var user = httpContextAccessor.HttpContext?.User;
            var userId = userManager.GetUserId(user);

            if (userId == null)
            {
                throw new Exception("User not found");
            }
            return userId;
        }
    }
}
