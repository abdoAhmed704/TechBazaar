using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Models;

namespace TechBazaar.Core.ModelViews
{
    public class CartFilterModelView
    {
        public CartStatus? Status { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }

        public IEnumerable<Cart> Orders { get; set; } = [];
    }
}
