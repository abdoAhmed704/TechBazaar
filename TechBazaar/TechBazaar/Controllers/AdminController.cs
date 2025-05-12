using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using TechBazaar.Core.Enums;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;
using TechBazaar.Ef.Repository;

namespace TechBazaar.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Orders(CartStatus? status, DateTime? from, DateTime? to)
        {
            var orders = unitOfWork.Cart.GetAll();

            if (status != null)
                orders = orders.Where(o => o.Status == status);

            if (from is not null)
                orders = orders.Where(o => o.CreatedAt >= from);

            if (to is not null)
                orders = orders.Where(o => o.CreatedAt <= to);

            var model = new CartFilterModelView
            {
                Status = status,
                From = from,
                To = to,
                Orders = orders
            };

            ViewBag.StatusList = new SelectList(Enum.GetNames(typeof(CartStatus)));


            return View(model);
        }

        public IActionResult OrderDetails(int id)
        {
            var cart = unitOfWork.Cart.GetCartById(id);
            if (cart == null)
                return NotFound();

            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateOrderStatus(int id, CartStatus status)
        {
            var cart = unitOfWork.Cart.GetCartById(id);
            if (cart == null)
                return NotFound();

            unitOfWork.Cart.UpdateCartStatus(id, status);
            unitOfWork.SaveChanges();

            return RedirectToAction("Orders");
        }

        public IActionResult SalesReport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalesReport(DateTime from, DateTime to)
        {
            var orders = await unitOfWork.Cart.GetOrderReport(from, to);

            var totalSales = orders.Sum(c => c.TotalPrice());
            var totalOrders = orders.Count();
            var averageOrderValue = totalOrders > 0 ? totalSales / totalOrders : 0;

            var report = new SalesReportModelView
            {
                From = from,
                To = to,
                TotalSales = totalSales,
                TotalOrders = totalOrders,
                AverageOrderValue = averageOrderValue
            };


            return View("SalesReportResult", report);
        }
    }
}

