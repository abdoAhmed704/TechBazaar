using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
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
        public Cart GetCartById(int cartId)
        {
            return eContext.Carts.Include(c => c.CartItems).Include(c => c.ApplicationUser).FirstOrDefault(c => c.Id == cartId);
        }

        public IEnumerable<Cart> GetCarts(CartStatus? status = null, DateTime? from = null, DateTime? to = null)
        {
            var query = eContext.Carts
                .Include(c => c.CartItems)
                .Include(c => c.ApplicationUser)
                .AsQueryable();

            if (status.HasValue)
            {
                query = query.Where(c => c.Status == status.Value);
            }

            if (from.HasValue)
            {
                query = query.Where(c => c.CreatedAt >= from.Value);
            }

            if (to.HasValue)
            {
                query = query.Where(c => c.CreatedAt <= to.Value);
            }

            return query.OrderByDescending(c => c.CreatedAt).ToList();
        }

        public void UpdateCartStatus(int cartId, CartStatus status)
        {
            var cart = eContext.Carts.Find(cartId);
            if (cart != null)
            {
                cart.Status = status;
                cart.ModifiedAt = DateTime.UtcNow;
                eContext.Update(cart);
            }
        }
        public async Task<int> GetTotalItemInCart()
        {
            var userId = GetUserId();
            var cart = await eContext.Set<T>().Include(c => c.CartItems).FirstOrDefaultAsync(c => c.UserId == userId && c.Status == CartStatus.Active && c.IsActive);
            var cartItemsCount = cart?.CartItems.Count();
            return cartItemsCount ?? 0;
        }
        public async Task<IEnumerable<Cart>> GetCustomerOrdersAsync()
        {
            var userId = GetUserId();
            var carts = await eContext.Carts
                .Where(c => c.UserId == userId && c.Status != CartStatus.Active)
                .ToListAsync();
            return carts;
        }

        public async Task<bool> AddToCart(int productId,int quantity)
        {
            var userId = GetUserId();
            var product = await eContext.Products.Include(p => p.ProductDiscounts).ThenInclude(pd => pd.Discount)
                .FirstOrDefaultAsync(p => p.Id == productId && p.Inventory.Quantity >= quantity);

            if (product == null)
            {
                return false;
            }

            decimal productPrice = product.Price;

            // Add Discount to the product price
            foreach(var productDiscount in product.ProductDiscounts)
            {
                var discount = productDiscount.Discount;
                if (discount != null && discount.IsActive) 
                {
                    if (discount.Type == DicscountType.Percentage)
                    {
                        productPrice -= ((discount.Value / productPrice) * 100);
                    }
                    else if (discount.Type == DicscountType.Fixed)
                    {
                        productPrice -= discount.Value;
                    }
                }
            }

            Cart cart =  eContext.Carts.Include(c => c.CartItems).FirstOrDefault(c => c.UserId == userId && c.IsActive && c.Status == CartStatus.Active);
                
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
                newCart.Total = newCart.TotalPrice();
                eContext.Carts.Add(newCart);
            }
            else
            {
                var cartItem = cart.CartItems.FirstOrDefault(ci => ci.ProductId == productId);
                

                if (cartItem != null)
                {
                    //if (product.Inventory.Quantity < (cartItem.Quantity += quantity)) return false;
                    cartItem.Quantity += quantity;
                }
                else
                {
                    cart.CartItems.Add(new CartItem { ProductId = productId, Quantity = quantity ,Price = product.Price,PriceAfterDiscount = productPrice});
                }
                cart.Total = cart.TotalPrice();

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
                if (cartItem != null && cartItem.Quantity > 1)
                {
                    cartItem.Quantity -=1;
                    eContext.CartItems.Update(cartItem);
                    cart.Total = cart.TotalPrice();
                    await eContext.SaveChangesAsync();
                }
                else
                {
                    cart.CartItems.Remove(cartItem);
                    cart.Total = cart.TotalPrice();
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
