using TechBazaar.Models;

namespace TechBazaar.Repository.Base
{
    public interface IUnitOfWork : IDisposable
    {
        IBaseRepository<Category> Category { get; }

        void SaveChanges();


    }
}
