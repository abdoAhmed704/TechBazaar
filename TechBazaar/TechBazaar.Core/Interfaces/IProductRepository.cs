using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IProductRepository<T> : IBaseRepository<T> where T : Product
    {
        Task<T> GetProductByIdAsync(int id);
        Task<IEnumerable<T>> GetAllProductsAsync();
        Task<T> GetProductByIdWithImages(int id);

    }
}
