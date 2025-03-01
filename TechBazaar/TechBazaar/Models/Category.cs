using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Desc { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        public ICollection<SubCategory> SubCategories { get; set; } = new List<SubCategory>();
    }
}
