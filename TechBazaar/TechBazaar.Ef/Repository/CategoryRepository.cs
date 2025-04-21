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
        public async Task<Category> GetCategoryByIdAsync(int id)
        {
            return await eContext.Set<Category>().FindAsync(id);
        }
        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            return await eContext.Set<Category>().ToListAsync();
        }
    }
}
