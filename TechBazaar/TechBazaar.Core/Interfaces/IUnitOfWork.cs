using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Category { get; }
        IBaseRepository<Product> Product { get; }

        void SaveChanges();
        Task SaveChangesAsync();


    }
}
