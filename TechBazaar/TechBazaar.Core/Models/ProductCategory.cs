using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class ProductCategory
    {
        [Key]
        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("SubCategory")]
        public int SubId { get; set; }

        public Product Product { get; set; }
        public SubCategory SubCategory { get; set; }
    }
}
