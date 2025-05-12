// Models/ViewModels/CheckoutViewModel.cs
using System.ComponentModel.DataAnnotations;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.ViewModels
{
    public class CheckoutViewModel
    {
        public int CartId { get; set; }
        public string UserFullName { get; set; }
        public string Address { get; set; }
        public decimal TotalAmount { get; set; }
        public List<PaymentMethod> PaymentMethods { get; set; }
        public int SelectedPaymentMethodId { get; set; }
    }
}
