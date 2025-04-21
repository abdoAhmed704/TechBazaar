using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IBrandRepository<T> : IBaseRepository<T> where T : Brand
    {
        Task<T> GetBrandByIdAsync(int id);
        Task<IEnumerable<T>> GetAllBrandsAsync();
        Task<IEnumerable<SelectListItem>> GetBrandsToSelectListItem();

    }
}
