using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class Shipment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [ForeignKey("ShippingProvider")]
        public int ProviderId { get; set; }

        [Required]
        [StringLength(100)]
        public string TrackingNumber { get; set; }

        [ForeignKey("ShipmentStatus")]
        public int StatusId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DeliveryDate { get; set; }

        public Order Order { get; set; }
        public ShippingProvider ShippingProvider { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; }
    }
}
