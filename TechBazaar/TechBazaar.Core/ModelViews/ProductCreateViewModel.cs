using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace TechBazaar.Core.ModelViews
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        public List<SelectListItem> Categories { get; set; }
        public List<IFormFile> ImageFiles { get; set; }
    }
}
