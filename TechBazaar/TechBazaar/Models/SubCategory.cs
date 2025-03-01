using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class SubCategory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Desc { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        // Navigation properties
        public Category Category { get; set; }
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();
    }
}
