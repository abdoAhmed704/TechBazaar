using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;
using Microsoft.AspNetCore.Mvc.Rendering;
using TechBazaar.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using TechBazaar.Ef;
using Microsoft.Data.SqlClient;
using System.Text.Json;
using System.Threading.Tasks;


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

            var products = await unitOfWork.Product.GetAllProductsAsync();

           return View(products);
        }

        public async Task<IActionResult> Details(int id)
        {

            var product = await unitOfWork.Product.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        public async Task<IActionResult> Create()
        {

            var viewModel = new ProductCreateViewModel
            {
                Categories = await unitOfWork.Category.GetCategoriesToSelectListItem(),
                Brands = await unitOfWork.Brand.GetBrandsToSelectListItem(),
                Discounts = await unitOfWork.Discount.GetDiscountsToSelectListItem()

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
                    ShortDescription = model.ShortDescription,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    BrandId = model.BrandId,
                    Images = new List<Image>()
                };
                // Handle inventory
                product.Inventory.Quantity = model.Quantity;

                // Handle discounts
                if (model.DiscountIds != null && model.DiscountIds.Any())
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
            model.Categories = await unitOfWork.Category.GetCategoriesToSelectListItem();

            model.Brands = await unitOfWork.Brand.GetBrandsToSelectListItem();

            model.Discounts = await unitOfWork.Discount.GetDiscountsToSelectListItem();

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
                ShortDescription = product.ShortDescription,
                Price = product.Price,
                CategoryId = product.CategoryId,
                BrandId = product.BrandId,
                DiscountIds = await unitOfWork.Product.GetProductDiscountsAsync(product.Id),
                Categories = await unitOfWork.Category.GetCategoriesToSelectListItem(),
                Brands = await unitOfWork.Brand.GetBrandsToSelectListItem(),
                Discounts = await unitOfWork.Discount.GetDiscountsToSelectListItem(),
                ExistingImages = product.Images.ToList()

            };

            return View(viewModel);
        }


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
                    product.ShortDescription = model.ShortDescription;
                    product.Price = model.Price;
                    product.CategoryId = model.CategoryId;
                    product.BrandId = model.BrandId;
                    product.IsActive = model.IsActive;

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
                        unitOfWork.ProductDiscount.Add(discount);
                    }

                    // Handle new image uploads
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
                    // Handle image deletions
                    if (model.ImagesToDelete != null && model.ImagesToDelete.Count > 0)
                    {
                        foreach (var imageId in model.ImagesToDelete)
                        {
                            var image = product.Images.FirstOrDefault(i => i.Id == imageId);
                            if (image != null)
                            {
                                var filePath = Path.Combine(webHostEnvironment.WebRootPath, image.ImageUrl.TrimStart('/'));
                                if (System.IO.File.Exists(filePath))
                                {
                                    System.IO.File.Delete(filePath);
                                }
                                product.Images.Remove(image);
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
                return RedirectToAction("Index");
            }

            model.Categories = await unitOfWork.Category.GetCategoriesToSelectListItem();
            model.Brands = await unitOfWork.Brand.GetBrandsToSelectListItem();
            model.Discounts = await unitOfWork.Discount.GetDiscountsToSelectListItem();
            model.ExistingImages = (List<Image>)await unitOfWork.Image
                .GetAllAsync(i => i.ProductId == id);

            return View(model);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var product = await unitOfWork.Product.GetProductByIdWithImages(id);

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
            product.DeletedAt = DateTime.Now;
            product.IsActive = false;

            unitOfWork.Product.Update(product);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> DisplayProduct(int id)
        {

            var productEntity = await unitOfWork.Product
                .Include(p => p.Category)
                .Include(p => p.Brand)
                .Include(p => p.Images)
                .Include(p => p.Inventory)
                .FirstOrDefaultAsync(p => p.Id == id);
            IEnumerable<int> wishListProductIds = await unitOfWork.WishList.GetWishListProductIds();




            if (productEntity == null)
                return NotFound();

            var viewModel = new ProductDetailsModelView
            {
                Id = productEntity.Id,
                Name = productEntity.Name,
                ShortDescription = productEntity.ShortDescription,
                Description = productEntity.Description,
                Price = productEntity.Price,
                CategoryId = productEntity.CategoryId,
                CategoryName = productEntity.Category?.Name, 
                BrandId = productEntity.BrandId,
                BrandName = productEntity.Brand?.Name,
                Quantity = productEntity.Inventory?.Quantity ?? 0,
                WishListProductIds = wishListProductIds,
                Images = productEntity.Images?.Select(img => img.ImageUrl).ToList() ?? new List<string>()
            };

            return View(viewModel); 
        }


    }


}



