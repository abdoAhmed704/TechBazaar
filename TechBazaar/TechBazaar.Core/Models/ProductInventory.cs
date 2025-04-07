using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechBazaar.Core.Models
{
    public class ProductInventory
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        [StringLength(50)]
        public int Quantity { get; set; } 


        public Product Product { get; set; }
    }
}
