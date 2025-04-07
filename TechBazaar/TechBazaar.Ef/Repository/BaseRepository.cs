using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;
using TechBazaar.Core.Interfaces;

namespace TechBazaar.Ef.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly EContext eContext;

        public BaseRepository(EContext eContext)
        {
            this.eContext = eContext;
        }

        public void Add(T item)
        {
            eContext.Set<T>().Add(item);
        }

        public void AddRange(IEnumerable<T> items)
        {
            eContext.Set<T>().AddRange();
        }

        public void Delete(T item)
        {
            eContext.Set<T>().Remove(item);
        }

        public void DeleteRange(IEnumerable<T> items)
        {
            eContext.Set<T>().RemoveRange(items);
        }

        public void Edit(T item)
        {
            eContext.Set<T>().Update(item);
        }

        public void EditRange(IEnumerable<T> items)
        {
            eContext.Set<T>().UpdateRange(items);
        }

        public IEnumerable<T> FindAll()
        {
            return eContext.Set<T>().ToList();
        }

        public T FindById(int id)
        {
            return eContext.Set<T>().Find(id);
        }

        public async Task<T> FindByIdAsync(int id)
        {
            return await eContext.Set<T>().FindAsync(id);
        }

        public void SaveChanges()
        {
            eContext.SaveChanges();
        }

        public T SelectOne(Expression<Func<T, bool>> match)
        {
            return eContext.Set<T>().SingleOrDefault(match);
        }
    }
}
