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
    public class DiscountRepository<T> : BaseRepository<T>,IDiscountRepository<T> where T : Discount
    {
        private readonly EContext eContext;
        public DiscountRepository(EContext eContext) : base(eContext)
        {
            this.eContext = eContext;
        }

        public async Task<IEnumerable<SelectListItem>> GetDiscountsToSelectListItem()
        {
            return await eContext.Set<T>().Where(d => d.IsActive == true && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
            .Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToListAsync();
        }

    }
}
