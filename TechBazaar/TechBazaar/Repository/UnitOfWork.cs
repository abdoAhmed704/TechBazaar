using Microsoft.EntityFrameworkCore;
using TechBazaar.Models;
using TechBazaar.Repository.Base;

namespace TechBazaar.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly EContext eContext;

        public IBaseRepository<Category> Category { get; private set; }

        public UnitOfWork(EContext eContext)
        {
            this.eContext = eContext;
            Category = new BaseRepository<Category>(eContext);
        }



        public void SaveChanges()
        {
            eContext.SaveChanges();
        }

        public void Dispose()
        {
            eContext.Dispose();
        }
    }
}
