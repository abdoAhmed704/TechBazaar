using System.ComponentModel.DataAnnotations;
using TechBazaar.Core.Enums;

namespace TechBazaar.Core.Models
{
    public class Discount
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public DicscountType Type { get; set; }

        [Required]
        public decimal Value { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ProductDiscount> Products { get; set; } = new List<ProductDiscount>();
        
    }
}
