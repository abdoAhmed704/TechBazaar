using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Cart")]
        public int OrderId { get; set; }

        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }

        public decimal Amount { get; set; }

        public PaymentMethod? PaymentMethod { get; set; }
        public Cart? Cart { get; set; }
    }
}
