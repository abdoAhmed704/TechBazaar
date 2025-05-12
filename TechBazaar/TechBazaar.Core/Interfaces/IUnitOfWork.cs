using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        ICategoryRepository<Category> Category { get; }
        IProductRepository<Product> Product { get; }
        public IDiscountRepository<Discount> Discount { get;}
        public IBaseRepository<ProductDiscount> ProductDiscount { get;}
        public IBaseRepository<Inventory> Inventory { get; }
        public IBaseRepository<Image> Image { get; }
        public IBrandRepository<Brand> Brand { get; }
        public ICartRepository<Cart> Cart { get; }
        public ICheckoutRepository Checkout { get;}
        public IWishListRepository<WishList> WishList{ get; }
        void SaveChanges();
        Task SaveChangesAsync();


    }
}
