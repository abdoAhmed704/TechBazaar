using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class ShippingProvider
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        public int Phone { get; set; }

        [Required]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [StringLength(200)]
        public string WebSite { get; set; }

        [Required]
        [StringLength(200)]
        public string TrackingUrl { get; set; }

        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();
    }
}
