using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechBazaar.Core.Interfaces;
using TechBazaar.Core.Models;
using TechBazaar.Core.ModelViews;

namespace TechBazaar.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUnitOfWork unitOfWork;

        public HomeController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task<IActionResult> Index(string sTerm = "", int categoryId = 0, int  brandId = 0)
        {
            IEnumerable<Product> products = await unitOfWork.Product.DisplayProducts(sTerm, categoryId, brandId);
            IEnumerable<Category> categories = await unitOfWork.Category.DisplayCategories();
            IEnumerable<Brand> brands = await unitOfWork.Brand.DisplayBrands();
            IEnumerable<int> wishListProductIds = await unitOfWork.WishList.GetWishListProductIds();

            var model = new ProductDisplayModelView
            {
                Products = products,
                WishListProductIds = wishListProductIds,
                Categories = categories,
                Brands = brands,
                STerm = sTerm,
                CategoryId = categoryId,
                BrandId = brandId
            };

            return View(model);
        }
        public async Task<IActionResult> Orders()
        {
            var orders = await unitOfWork.Cart.GetCustomerOrdersAsync();
            return View(orders);
        }
    }
}
