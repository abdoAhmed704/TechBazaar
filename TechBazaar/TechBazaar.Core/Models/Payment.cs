using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("ApplicationUser")]
        public string UserId { get; set; }

        [ForeignKey("Cart")]
        public int OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [ForeignKey("PaymentStatus")]
        public int StatusId { get; set; }

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

        public ApplicationUser? ApplicationUser { get; set; }
        public Cart Cart { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
