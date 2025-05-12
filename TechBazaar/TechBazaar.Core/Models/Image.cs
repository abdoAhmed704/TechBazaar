using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product"),Required]
        public int ProductId { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public Product? Product { get; set; }
    }
}
