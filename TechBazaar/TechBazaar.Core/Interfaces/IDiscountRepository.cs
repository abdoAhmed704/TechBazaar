using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IDiscountRepository<T> : IBaseRepository<T> where T : Discount
    {
        Task<IEnumerable<SelectListItem>> GetDiscountsToSelectListItem();
    }
}
