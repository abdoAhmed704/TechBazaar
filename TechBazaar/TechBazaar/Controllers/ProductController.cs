using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechBazaar.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using TechBazaar.Ef;
using Microsoft.Data.SqlClient;
using System.Text.Json;


namespace TechBazaar.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment webHostEnvironment;
        private readonly IUnitOfWork unitOfWork;

        public ProductController(IWebHostEnvironment webHostEnvironment, IUnitOfWork unitOfWork )
        {
            this.webHostEnvironment = webHostEnvironment;
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index()
        {

           var products = await unitOfWork.Product
               .Include(p => p.Category)
               .ToListAsync();

           return View(products);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await unitOfWork.Product
                .Include(p => p.Category)
                .Include(p => p.Images)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public IActionResult Create()
        {

            var viewModel = new ProductCreateViewModel
            {
                Categories = unitOfWork.Category
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList(),
                Discounts = unitOfWork.Discount
                .GetAll(d => d.IsActive == true && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                .Select(d => new SelectListItem
                {
                    Value = d.Id.ToString(),
                    Text = d.Name
                }).ToList()

            };
            return View(viewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = new Product
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    Images = new List<Image>()
                };
                // Handle discounts
                if (model.DiscountIds != null && model.DiscountIds.Count > 0)
                {
                    foreach (var discountId in model.DiscountIds)
                    {
                        var productDiscount = new ProductDiscount
                        {
                            ProductId = product.Id,
                            DiscountId = discountId
                        };
                        product.ProductDiscounts.Add(productDiscount);
                    }
                }

                // Handle image uploads
                if (model.ImageFiles != null && model.ImageFiles.Count > 0)
                {
                    string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "ProductImages");
                    Directory.CreateDirectory(uploadsFolder);

                    foreach (var file in model.ImageFiles)
                    {
                        if (file.Length > 0)
                        {
                            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                            string filePath = Path.Combine(uploadsFolder, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                await file.CopyToAsync(stream);
                            }

                            product.Images.Add(new Image
                            {
                                ImageUrl = "/ProductImages/" + fileName
                            });
                        }
                    }
                }

                unitOfWork.Product.Add(product);
                await unitOfWork.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // If model state is invalid, repopulate categories
            model.Categories = unitOfWork.Category
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View(model);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await unitOfWork.Product
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            var viewModel = new ProductCreateViewModel
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CategoryId = product.CategoryId,
                Categories = unitOfWork.Category
                    .Select(c => new SelectListItem
                    {
                        Value = c.Id.ToString(),
                        Text = c.Name
                    }).ToList(),
                Discounts = unitOfWork.Discount
                    .GetAll(d => d.IsActive == true && d.StartDate <= DateTime.Now && d.EndDate >= DateTime.Now)
                    .Select(d => new SelectListItem
                    {
                        Value = d.Id.ToString(),
                        Text = d.Name
                    }).ToList()
            };
            // Store existing images in TempData for use in the view

            TempData["ExistingImages"] = JsonSerializer.Serialize(product.Images.Select(i => new { i.Id, i.ImageUrl }).ToList());
            return View(viewModel);
        }

        // POST: Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var product = await unitOfWork.Product
                        .Include(p => p.Images)
                        .FirstOrDefaultAsync(p => p.Id == id);

                    if (product == null)
                    {
                        return NotFound();
                    }

                    product.Name = model.Name;
                    product.Description = model.Description;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;

                    // Handle discounts
                    var currentDicounts = unitOfWork.ProductDiscount.GetAll(p => p.ProductId == product.Id).ToList();
                    unitOfWork.ProductDiscount.DeleteRange(currentDicounts);

                    foreach (var discountId in model.DiscountIds)
                    {
                        var discount = new ProductDiscount
                        {
                            ProductId = product.Id,
                            DiscountId = discountId
                        };
                    }

                    // Handle new image uploads
                    if (model.ImageFiles != null && model.ImageFiles.Count > 0)
                    {
                        string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "uploads");
                        Directory.CreateDirectory(uploadsFolder);

                        foreach (var file in model.ImageFiles)
                        {
                            if (file.Length > 0)
                            {
                                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                                string filePath = Path.Combine(uploadsFolder, fileName);

                                using (var stream = new FileStream(filePath, FileMode.Create))
                                {
                                    await file.CopyToAsync(stream);
                                }

                                product.Images.Add(new Image
                                {
                                    ImageUrl = "/uploads/" + fileName
                                });
                            }
                        }
                    }

                    unitOfWork.Product.Update(product);
                    await unitOfWork.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!unitOfWork.Product.Exists(id))
                    {
                        return NotFound();
                    }
                    throw;
                }
                return RedirectToAction(nameof(Index));
            }

            model.Categories = unitOfWork.Category
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name
                }).ToList();

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await unitOfWork.Product
                .Include(p => p.Images)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            // Delete associated images from server
            foreach (var image in product.Images)
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }

            unitOfWork.Product.Delete(product);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }


}



