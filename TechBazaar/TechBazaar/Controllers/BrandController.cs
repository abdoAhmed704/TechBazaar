using Microsoft.AspNetCore.Mvc;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;
using System.IO;
using System.Threading.Tasks;

namespace TechBazaar.Controllers
{
    public class BrandController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public BrandController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }
        public async Task<IActionResult> Index()
        {
            var brands = await unitOfWork.Brand.GetAllBrandsAsync();
            return View(brands);
        }
        public async Task<IActionResult> Details(int id)
        {
            var brand = await unitOfWork.Brand.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BrandModelView brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            var newBrand = new Brand
            {
                Name = brand.Name,
                IsActive = brand.IsActive,
            };
            if (brand.ImageFile is not null)
            {
                // Save the image to the server
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                Directory.CreateDirectory(uploadsFolder);

                var FileName = Guid.NewGuid().ToString() + Path.GetExtension(brand.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await brand.ImageFile.CopyToAsync(stream);
                }

                // Save the file name to the database
                newBrand.ImageUrl = "/BrandImages/" + FileName;
            }
            unitOfWork.Brand.Add(newBrand);
            await unitOfWork.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await unitOfWork.Brand.GetBrandByIdAsync(id);

            if (brand == null)
            {
                return NotFound();
            }

            var brandModelView = new BrandModelView
            {
                Id = brand.Id,
                Name = brand.Name,
                CurrentImageUrl = brand.ImageUrl,
                IsActive = brand.IsActive
            };

            return View(brandModelView);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, BrandModelView brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }
            var existingBrand = await unitOfWork.Brand.GetBrandByIdAsync(id);
            if (existingBrand == null)
            {
                return NotFound();
            }
            existingBrand.Name = brand.Name;
            existingBrand.IsActive = brand.IsActive;
            existingBrand.ModifiedAt = DateTime.UtcNow;

            if (brand.ImageFile is not null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingBrand.ImageUrl))
                {
                    var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, existingBrand.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the image to the server
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "BrandImages");
                Directory.CreateDirectory(uploadsFolder);
                var FileName = Guid.NewGuid().ToString() + Path.GetExtension(brand.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await brand.ImageFile.CopyToAsync(stream);
                }

                // Save the file name to the database
                existingBrand.ImageUrl = "/BrandImages/" + FileName;
            }

            unitOfWork.Brand.Update(existingBrand);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await unitOfWork.Brand.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            return View(brand);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirm(int id)
        {
            var brand = await unitOfWork.Brand.GetBrandByIdAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            // Delete the image file if it exists
            if (!string.IsNullOrEmpty(brand.ImageUrl))
            {
                var imagePath = Path.Combine(webHostEnvironment.WebRootPath, brand.ImageUrl.TrimStart('/'));
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }
            unitOfWork.Brand.Delete(brand);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
