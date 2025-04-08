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
