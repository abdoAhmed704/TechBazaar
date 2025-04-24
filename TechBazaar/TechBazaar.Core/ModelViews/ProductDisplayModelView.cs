using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.ModelViews
{
    public class ProductDisplayModelView
    {
        public IEnumerable<Product> Products { get; set; } = [];
        public IEnumerable<Category> Categories { get; set; } = [];
        public IEnumerable<Brand> Brands { get; set; } = [];

        public string SearchTerm { get; set; } = "";
        public int CategoryId { get; set; } = 0;
        public int BrandId { get; set; } = 0;
    }
}
