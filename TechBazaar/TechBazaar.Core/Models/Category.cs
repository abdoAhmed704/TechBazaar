using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string? Name { get; set; }

        public string? Description { get; set; }

        public string? ImageUrl { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedAt { get; set; }

        public bool IsActive { get; set; } = true;


        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
