using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.Models
{
    public class ProductDiscount
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [ForeignKey("Discount")]
        public int DiscountId { get; set; }

        public Product Product { get; set; }
        public Discount Discount { get; set; }
    }
}
