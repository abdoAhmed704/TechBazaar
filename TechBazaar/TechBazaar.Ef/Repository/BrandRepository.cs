using Microsoft.AspNetCore.Mvc.Rendering;
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

        public async Task<T> GetBrandByIdAsync(int id)
        {
            return await eContext.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetAllBrandsAsync()
        {
            return await eContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetBrandsToSelectListItem()
        {
            return await eContext.Set<T>().Where(b => b.IsActive == true).Select(b => new SelectListItem
            {
                Value = b.Id.ToString(),
                Text = b.Name
            }).ToListAsync();
        }
    }
}
