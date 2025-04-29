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
            var product = await eContext.Products.Include(p => p.ProductDiscounts).ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Id == productId && p.Inventory.Quantity >= quantity);
            decimal productPrice = product.Price;
            // Add Discount to the product price
            foreach(var productDiscount in product.ProductDiscounts)
            {
                var discount = productDiscount.Discount;
                if (discount != null && discount.IsActive) 
                {
                    if (discount.Type == DicscountType.Percentage)
                    {
                        productPrice -= productPrice * ((discount.Value / 100)* 100);
                    }
                    else if (discount.Type == DicscountType.Fixed)
                    {
                        productPrice -= discount.Value;
                    }
                }
            }
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
                newCart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity,Price = product.Price,PriceAfterDiscount = productPrice });
                eContext.Carts.Add(newCart);
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
                    cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity ,Price = product.Price,PriceAfterDiscount = productPrice});
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

            var cart = await eContext.Set<T>()
    .Include(c => c.CartItems)
        .ThenInclude(ci => ci.Product)
            .ThenInclude(p => p.Inventory)
        .Include(c => c.CartItems)
            .ThenInclude(ci => ci.Product)
                .ThenInclude(p => p.Category)
    .FirstOrDefaultAsync(c => c.UserId == userId && c.IsActive);

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
