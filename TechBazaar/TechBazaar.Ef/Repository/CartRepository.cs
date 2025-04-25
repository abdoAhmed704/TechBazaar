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
    public class CartRepository<T> : BaseRepository<T>, ICartRepository<T> where T : Cart
    {
        private readonly EContext eContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public CartRepository(EContext eContext,IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager):base(eContext)
        {
            this.eContext = eContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
        }
        public async Task<int> GetTotalItemInCart()
        {
            var userId = GetUserId();
            var cart = await eContext.Set<T>().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId);
            var cartItemsCount = cart?.CartItems.Count();
            return cartItemsCount ?? 0;
        }
        public async Task AddToCart(int productId,int quantity)
        {
            var userId = GetUserId();
            var product = eContext.Products.FirstOrDefaultAsync(p => p.Id == productId && p.Inventory.Quantity >= 0);
            if (product == null)
            {
                throw new Exception("Product not found or insufficient inventory");
            }
            var cart = await eContext.Set<T>().Include(c => c.CartItems).
                FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive && c.Status == CartStatus.Active);
            if (cart == null)
            {
                var newCart = new Cart
                {
                    UserId = userId,
                    CreatedAt = DateTime.Now,
                    Status = CartStatus.Active,
                    RefNumber = Guid.NewGuid().ToString(),
                    IsActive = true
                };
                newCart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
                await eContext.Set<T>().AddAsync(cart);
            }
            else
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity });
                }
            }
            await eContext.SaveChangesAsync();
        }
        public async Task RemoveFromCart(int productId)
        {
            var userId = GetUserId();
            var cart = await eContext.Set<T>().Include(c => c.CartItems).
                FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive && c.Status == CartStatus.Active);
            if (cart != null)
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                if (cartItem != null)
                {
                    cart.CartItems.Remove(cartItem);
                    await eContext.SaveChangesAsync();
                }
            }
        }
        public async Task<T> GetUserCart() {
            var userId = GetUserId();

            var cart = await eContext.Set<T>().Include(c => c.CartItems).
                FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);
           
            return cart;
        }

        public async Task<T> GetCart(string userId)
        {
            var cart = await eContext.Set<T>().FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
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
