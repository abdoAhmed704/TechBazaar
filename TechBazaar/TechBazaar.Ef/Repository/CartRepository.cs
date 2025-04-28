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
            var cart = await eContext.Set<T>().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active && c.IsActive);
            var cartItemsCount = cart?.CartItems.Count();
            return cartItemsCount ?? 0;
        }
        public async Task<bool> AddToCart(int productId,int quantity)
        {
            var userId = GetUserId();
            var product = await eContext.Products.FirstOrDefaultAsync(p => p.Id == productId && p.Inventory.Quantity >= quantity);
            if (product == null)
            {
                return false;
            }
            Cart cart =  eContext.Carts.FirstOrDefault(c => c.UserId == userId && c.IsActive && c.Status == CartStatus.Active);
                
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
                await eContext.Carts.AddAsync(cart);
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
                    cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity ,Price = product.Price });
                }
            }
            await eContext.SaveChangesAsync();
            return true;
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
