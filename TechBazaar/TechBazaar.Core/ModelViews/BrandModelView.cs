using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.ModelViews
{
    public class BrandModelView
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? CurrentImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;

    }
}
