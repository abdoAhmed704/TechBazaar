using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        public DateTime ModifiedAt { get; set; }

        // Navigation properties
        public ICollection<UserAddress> UserAddresses { get; set; } = new List<UserAddress>();
        public ICollection<UserPayment> UserPayments { get; set; } = new List<UserPayment>();
        public ICollection<Order> Orders { get; set; } = new List<Order>();
        public ICollection<WishList> WishLists { get; set; } = new List<WishList>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
