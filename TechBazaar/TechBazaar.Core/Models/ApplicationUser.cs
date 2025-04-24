using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class ApplicationUser:IdentityUser
    {
        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedAt { get; set; }

        public bool IsActive { get; set; } = true;


        [StringLength(100)]

        public string? Street { get; set; }
        public int BuldingNo { get; set; }
        public int Floor { get; set; }
        public int AppartmentNo { get; set; }

        [StringLength(50)]
        public string? City { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        [StringLength(20)]
        public int PostalCode { get; set; }

        // Navigation properties
        public WishList WishList { get; set; }
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
        public ICollection<Cart> Carts { get; set; } = new List<Cart>();
    }
}
