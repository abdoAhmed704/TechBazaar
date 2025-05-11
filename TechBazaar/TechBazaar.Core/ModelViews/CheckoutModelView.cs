// Models/ViewModels/CheckoutViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.ViewModels
{
    public class CheckoutViewModel
    {
        [Required]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public int BuildingNo { get; set; }

        public int Floor { get; set; }

        public int AppartmentNo { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string PostalCode { get; set; }

        public decimal OrderTotal { get; set; }
        public string? Message { get; set; }
        public string? CartItems { get; set; }
    }
}
