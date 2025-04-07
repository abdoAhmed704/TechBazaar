using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class UserPayment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string PaymentMethod { get; set; }

        [Required]
        [StringLength(20)]
        public int CardNumber { get; set; }

        [Required]
        [StringLength(10)]
        [DataType(DataType.Date)]
        public string ExpireDate { get; set; }

        [Required]
        [StringLength(4)]
        public string CVV { get; set; }

        [Required]
        public bool IsDefault { get; set; }

        public ApplicationUser? ApplicationUser { get; set; }
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
