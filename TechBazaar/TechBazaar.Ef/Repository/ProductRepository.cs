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
    public class ProductRepository<T>: BaseRepository<T>,IProductRepository<T> where T : Product
    {
        private readonly EContext eContext;

        public ProductRepository(EContext eContext) : base(eContext)
        {
            this.eContext = eContext;
        }

        public async Task<T> GetProductByIdAsync(int id)
        {
            return await eContext.Set<T>().Include(p => p.Category).
                Include(p => p.Brand).Include(p => p.Inventory).Include(p => p.Images).FirstOrDefaultAsync(p =>p.Id == id && p.DeletedAt == null);
        }

        public async Task<T> GetProductByIdWithImages(int id)
        {
            return await eContext.Set<T>().Include(p => p.Images).FirstOrDefaultAsync(p => p.DeletedAt == null && p.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllProductsAsync()
        {
            return await eContext.Set<T>().Include(p => p.Category).
                Include(p => p.Brand).Where(p => p.DeletedAt == null).ToListAsync();
        }

        public async Task<IEnumerable<T>> DisplayProducts(string sTearm = "",int categoryId = 0,int brandId = 0)
        {
            var products = eContext.Set<T>().AsNoTracking().Include(p => p.Category).
                Include(p => p.Brand).Include(p =>p.Images).Include(p=>p.Inventory).
                Where(p => p.DeletedAt == null && p.IsActive == true);
            if (!string.IsNullOrEmpty(sTearm))
            {
                products = products.Where(p => p.Name.Contains(sTearm));
            }
            if (categoryId > 0)
            {
                products = products.Where(p => p.CategoryId == categoryId);
            }
            if (brandId > 0)
            {
                products = products.Where(p => p.BrandId == brandId);
            }
            return await products.ToListAsync();
        }

        
    }
}
