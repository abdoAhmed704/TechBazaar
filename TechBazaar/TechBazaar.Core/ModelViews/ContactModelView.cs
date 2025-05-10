// Models/ViewModels/ContactViewModel.cs
using System.ComponentModel.DataAnnotations;

namespace TechBazaar.Core.ViewModels
{
    public class ContactViewModel
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [StringLength(100)]
        public string Subject { get; set; }

        [Required]
        [StringLength(1000)]
        public string Message { get; set; }
    }
}
