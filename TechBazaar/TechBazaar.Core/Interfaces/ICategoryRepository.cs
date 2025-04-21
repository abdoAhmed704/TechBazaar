using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface ICategoryRepository<T> : IBaseRepository<T> where T : Category
    {
        Task<T> GetCategoryByIdAsync(int id);
        Task<IEnumerable<T>> GetAllCategoriesAsync();
        Task<IEnumerable<SelectListItem>> GetCategoriesToSelectListItem();
    }
}
