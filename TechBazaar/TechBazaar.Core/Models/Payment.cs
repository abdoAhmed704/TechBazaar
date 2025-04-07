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

        [ForeignKey("UserPayment")]
        public int UserPaymentId { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [ForeignKey("PaymentStatus")]
        public int StatusId { get; set; }


        public ApplicationUser? ApplicationUser { get; set; }
        public UserPayment? UserPayment { get; set; }
        public Order? Order { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    }
}
