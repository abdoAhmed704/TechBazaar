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
    internal class BrandRepository<T> : BaseRepository<T>, IBrandRepository<T> where T : Brand
    {
        private readonly EContext eContext;
        public BrandRepository(EContext eContext) : base(eContext)
        {
            this.eContext = eContext;
        }
        public async Task<Brand> GetBrandByIdAsync(int id)
        {
            return await eContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<Brand>> GetAllBrandsAsync()
        {
            return await eContext.Set<T>().ToListAsync();
        }
    }
}
