using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class WishList
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [Required]
        public DateTime ModifiedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<WishItem> WishItems { get; set; } = new List<WishItem>();
    }
}
