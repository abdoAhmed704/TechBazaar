using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.ModelViews
{
    public class CategoryModelView
    {
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required,StringLength(500)]
        public string? Description { get; set; }

        public string? CurrentImageUrl { get; set; }
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
