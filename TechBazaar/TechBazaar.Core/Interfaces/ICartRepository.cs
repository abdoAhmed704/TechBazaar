using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface ICartRepository<T> : IBaseRepository<T> where T : Cart
    {
        Task<int> GetTotalItemInCart();
        Task<bool> AddToCart(int productId, int quantity);
        Task RemoveFromCart(int productId);
        Task<T> GetUserCart();
        Task<T> GetCart(string userId);
        Task GetTotalItemInWishList();
    }
}
