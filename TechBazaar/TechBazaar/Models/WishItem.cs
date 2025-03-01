using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class WishItem
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("WishList")]
        public int WishId { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }

        public WishList WishList { get; set; }
        public Product Product { get; set; }
    }
}
