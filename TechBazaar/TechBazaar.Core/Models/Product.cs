using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }


        public Inventory Inventory { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public ICollection<WishItem> WishItems { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Image> Images { get; set; }

        public Category Category { get; set; }
        public ICollection<ProductDiscount> ProductDiscounts { get; set; }
    }
}
