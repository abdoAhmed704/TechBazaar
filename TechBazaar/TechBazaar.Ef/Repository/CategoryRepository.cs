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
    public class CategoryRepository<T> : BaseRepository<T>, ICategoryRepository<T> where T : Category
    {
        private readonly EContext eContext;
        public CategoryRepository(EContext eContext) : base(eContext)
        {
            this.eContext = eContext;
        }
        public async Task<T> GetCategoryByIdAsync(int id)
        {
            return await eContext.Set<T>().FindAsync(id);
        }
        public async Task<IEnumerable<T>> GetAllCategoriesAsync()
        {
            return await eContext.Set<T>().ToListAsync();
        }
        public async Task<IEnumerable<SelectListItem>> GetCategoriesToSelectListItem()
        {
            return await eContext.Set<T>().Where(c => c.IsActive == true).Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
        }
    }
}
