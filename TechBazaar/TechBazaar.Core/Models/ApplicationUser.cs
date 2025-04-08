using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
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


        [Required]
        [StringLength(100)]
        public string AddressLine1 { get; set; }

        [StringLength(100)]
        public string AddressLine2 { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; }

        [Required]
        [StringLength(20)]
        public int PostalCode { get; set; }

        [Required]
        public string Phone { get; set; }

        // Navigation properties
        public ICollection<WishList> WishLists { get; set; } = new List<WishList>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
