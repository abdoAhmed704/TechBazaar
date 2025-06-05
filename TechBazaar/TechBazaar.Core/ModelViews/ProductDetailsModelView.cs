using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.ModelViews
{
    public class ProductDetailsModelView
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int BrandId { get; set; }
        public string BrandName { get; set; }

        public int Quantity { get; set; }
        public Product Product { get; set; }

        public IEnumerable<int> WishListProductIds { get; set; } = [];

        public List<string> Images { get; set; } = new List<string>();
    }

}
