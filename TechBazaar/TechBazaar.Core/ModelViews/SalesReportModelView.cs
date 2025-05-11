using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TechBazaar.Core.ModelViews
{
    public class SalesReportModelView
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal TotalSales { get; set; }
        public int TotalOrders { get; set; }
        public decimal AverageOrderValue { get; set; }
    }
}
