using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface ICheckoutRepository
    {
        List<PaymentMethod> GetActivePaymentMethods();
        void AddPayment(Payment payment);
        void UpdateCartStatus(Cart cart, CartStatus status);
    }
}
