using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment webHostEnvironment;

        public CategoryController(IUnitOfWork unitOfWork,IWebHostEnvironment webHostEnvironment)
        {
            this.unitOfWork = unitOfWork;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            return View(await unitOfWork.Category.GetAllCategoriesAsync());
        }

        public async Task<IActionResult> Details(int id)
        {

            var category = await unitOfWork.Category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        public IActionResult Create()
        {
            return View(new CategoryModelView());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryModelView category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var newCategory = new Category
            {
                Name = category.Name,
                Description = category.Description,
                CreatedAt = DateTime.UtcNow,
                IsActive = category.IsActive,
            };
            if (category.ImageFile != null)
            {
                // Save the image to the server
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "CategoryImages");
                Directory.CreateDirectory(uploadsFolder);

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(stream);
                }

                // Save the file name to the database
                newCategory.ImageUrl = "/CategoryImages/" + fileName;
            }

            unitOfWork.Category.Add(newCategory);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await unitOfWork.Category.GetCategoryByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            var categoryModelView = new CategoryModelView
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description,
                IsActive = category.IsActive,
                CurrentImageUrl = category.ImageUrl
            };
            return View(categoryModelView);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryModelView category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }
            var existingCategory = await unitOfWork.Category.GetCategoryByIdAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }
            existingCategory.Name = category.Name;
            existingCategory.IsActive = category.IsActive;
            existingCategory.Description = category.Description;
            existingCategory.ModifiedAt = DateTime.UtcNow;

            if (category.ImageFile is not null)
            {
                // Delete the old image if it exists
                if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                {
                    var oldImagePath = Path.Combine(webHostEnvironment.WebRootPath, existingCategory.ImageUrl.TrimStart('/'));
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                // Save the image to the server
                var uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "CategoryImages");
                Directory.CreateDirectory(uploadsFolder);
                var FileName = Guid.NewGuid().ToString() + Path.GetExtension(category.ImageFile.FileName);
                var filePath = Path.Combine(uploadsFolder, FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await category.ImageFile.CopyToAsync(stream);
                }

                // Save the file name to the database
                existingCategory.ImageUrl = "/CategoryImages/" + FileName;
            }

            unitOfWork.Category.Update(existingCategory);
            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var category = await unitOfWork.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await unitOfWork.Category.GetCategoryByIdAsync(id);
            if (category != null)
            {
                unitOfWork.Category.Delete(category);
            }

            await unitOfWork.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
