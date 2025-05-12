using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.ModelViews
{
    public class ProductCreateViewModel
    {
        public string Name { get; set; }

        [StringLength(100)]
        public string ShortDescription { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public bool IsActive { get; set; } = true;
        public IEnumerable<int> DiscountIds { get; set; } = new List<int>();
        public IEnumerable<SelectListItem> Discounts { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Brands { get; set; } = new List<SelectListItem>();
        public List<Image> ExistingImages { get; set; } = new List<Image>();
        public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();
        public List<int> ImagesToDelete { get; set; } = new List<int>();
    }
}
