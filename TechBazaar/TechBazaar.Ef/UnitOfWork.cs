using TechBazaar.Core.Models;
using TechBazaar.Core.Interfaces;
using TechBazaar.Ef.Repository;
using TechBazaar.Ef;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace TechBazaar.Ef
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EContext eContext;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<ApplicationUser> userManager;

        public ICategoryRepository<Category> Category { get; private set; }

        public IProductRepository<Product> Product { get; private set; }
        public IDiscountRepository<Discount> Discount { get; private set; }
        public IBaseRepository<ProductDiscount> ProductDiscount { get; private set; }
        public IBaseRepository<Inventory> Inventory { get; private set; }
        public IBaseRepository<Image> Image { get; set; }
        public IBrandRepository<Brand> Brand { get; private set; }
        public IWishListRepository<WishList> WishList { get; private set; }
        public ICartRepository<Cart> Cart { get; private set; }
        public ICheckoutRepository Checkout { get; private set; }


        public UnitOfWork(EContext eContext, IHttpContextAccessor httpContextAccessor,UserManager<ApplicationUser> userManager)
        {
            this.eContext = eContext;
            this.httpContextAccessor = httpContextAccessor;
            this.userManager = userManager;
            Category = new CategoryRepository<Category>(eContext);
            Product = new ProductRepository<Product>(eContext);
            Discount = new DiscountRepository<Discount>(eContext);
            ProductDiscount = new BaseRepository<ProductDiscount>(eContext);
            Inventory = new BaseRepository<Inventory>(eContext);
            Image = new BaseRepository<Image>(eContext);
            Brand = new BrandRepository<Brand>(eContext);
            WishList = new WishListRepository<WishList>(eContext, httpContextAccessor, userManager);
            Cart = new CartRepository<Cart>(eContext, httpContextAccessor, userManager);
            Checkout = new CheckoutRepository(eContext);

        }



        public void SaveChanges()
        {
            eContext.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await eContext.SaveChangesAsync();
        }
        public void Dispose()
        {
            eContext.Dispose();
        }
    }
}
