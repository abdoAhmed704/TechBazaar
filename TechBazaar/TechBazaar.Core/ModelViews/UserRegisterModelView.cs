using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.ModelViews
{
    public class UserRegisterModelView
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please confirm your password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }
        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        public string? PhoneNumber { get; set; }

        [StringLength(100)]
        public string? Street { get; set; }

        [Range(1, 10000, ErrorMessage = "Building number must be between 1 and 10000.")]
        public int BuldingNo { get; set; }

        [Range(0, 200, ErrorMessage = "Floor must be between 0 and 200.")]
        public int Floor { get; set; }

        [Range(1, 1000, ErrorMessage = "Apartment number must be between 1 and 1000.")]
        public int AppartmentNo { get; set; }

        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters.")]
        public string? City { get; set; }

        [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters.")]
        public string? Country { get; set; }

        [RegularExpression(@"^\d{4,20}$", ErrorMessage = "Postal code must be between 4 and 20 digits.")]
        public string? PostalCode { get; set; }
    }
}
