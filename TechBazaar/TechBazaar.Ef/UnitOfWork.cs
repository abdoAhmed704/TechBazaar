using TechBazaar.Core.Models;
using TechBazaar.Core.Interfaces;
using TechBazaar.Ef.Repository;
using TechBazaar.Ef;

namespace TechBazaar.Ef
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EContext eContext;

        public IBaseRepository<Category> Category { get; private set; }

        public IBaseRepository<Product> Product { get; private set; }
        public IBaseRepository<Discount> Discount { get; private set; }
        public IBaseRepository<ProductDiscount> ProductDiscount { get; private set; }
        public IBaseRepository<Inventory> Inventory { get; private set; }

        public UnitOfWork(EContext eContext)
        {
            this.eContext = eContext;
            Category = new BaseRepository<Category>(eContext);
            Product = new BaseRepository<Product>(eContext);
            Discount = new BaseRepository<Discount>(eContext);
            ProductDiscount = new BaseRepository<ProductDiscount>(eContext);
            Inventory = new BaseRepository<Inventory>(eContext);
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
