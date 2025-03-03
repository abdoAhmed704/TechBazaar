using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class ProductDiscount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        [StringLength(500)]
        public string Desc { get; set; }

        [Required]
        public int DiscountPercintage { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? ModifiedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeletedAt { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
