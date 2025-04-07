using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.Models
{
    public class ShipmentStatus
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }
        public ICollection<Shipment> Shipments { get; set; } = new List<Shipment>();

    }
}
