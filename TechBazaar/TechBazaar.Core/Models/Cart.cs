using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using TechBazaar.Core.Enums;

namespace TechBazaar.Core.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser"),Required]
        public string? UserId { get; set; }

        [Required]
        public decimal Total { get; set; }

        [Required]
        public CartStatus Status { get; set; }

        public string RefNumber { get; set; } =  Guid.NewGuid().ToString();

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime ModifiedAt { get; set; }

        public bool IsActive { get; set; } = true;

        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

        public decimal TotalPrice()
        {
            decimal total = 0;
            total = CartItems.Sum(item => item.TotalPrice());
            return total;
        }
    }
}
