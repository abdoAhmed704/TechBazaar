using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;

namespace TechBazaar.Ef.Repository
{
    public class CheckoutRepository : ICheckoutRepository
    {
        private readonly EContext eContext;

        public CheckoutRepository(EContext eContext)
        {
            this.eContext = eContext;
        }

        public List<PaymentMethod> GetActivePaymentMethods()
        {
            return eContext.PaymentMethods.Where(pm => pm.IsActive).ToList();
        }

        public void AddPayment(Payment payment)
        {
            eContext.Payments.Add(payment);
        }

        public void UpdateCartStatus(Cart cart, CartStatus status)
        {
            cart.Status = status;
            cart.IsActive = false;
            cart.ModifiedAt = DateTime.Now;
        }
    }
}
