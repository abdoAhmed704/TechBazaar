using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;
using TechBazaar.Ef;

namespace TechBazaar.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public InventoryController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public  IActionResult Index()
        {
            var ProductInventory = unitOfWork.Product.Select(p => new ProductInventoryModelView
            {
                Id = p.Inventory.Id,
                ProductName = p.Name,
                Quantity = p.Inventory.Quantity

            }).ToList();

            return View(ProductInventory);
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Inventory = unitOfWork.Inventory.Include(i => i.Product).FirstOrDefault(i => i.Id == id);
            if (Inventory == null) return NotFound();
            var productInventory = new ProductInventoryModelView
            {
                Id = Inventory.Id,
                ProductName = Inventory.Product.Name,
                Quantity = Inventory.Quantity
            };
            return View(productInventory);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ProductInventoryModelView model)
        {
            var inventory = unitOfWork.Inventory.Find(model.Id);

            if (inventory == null)
            {
                return View();
            }

            inventory.Quantity = model.Quantity;
            unitOfWork.Inventory.Update(inventory);
            unitOfWork.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
    }
}
