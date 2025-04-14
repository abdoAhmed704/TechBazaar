using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Category { get; }
        IBaseRepository<Product> Product { get; }
        public IBaseRepository<Discount> Discount { get;}
        public IBaseRepository<ProductDiscount> ProductDiscount { get;}
        public IBaseRepository<Inventory> Inventory { get; }
        public IBaseRepository<Image> Image { get; }
        void SaveChanges();
        Task SaveChangesAsync();


    }
}
