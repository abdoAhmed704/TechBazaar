using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime ModifiedAt { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}
