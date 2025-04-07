using TechBazaar.Core.Models;

namespace TechBazaar.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Category { get; }

        void SaveChanges();


    }
}
