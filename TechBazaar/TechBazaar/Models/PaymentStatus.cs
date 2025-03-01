using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Models
{
    public class PaymentStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // Assuming 'int' was a typo in schema

        // Navigation property
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
