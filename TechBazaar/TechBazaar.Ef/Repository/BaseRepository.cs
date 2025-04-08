using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TechBazaar.Core.Models;
using TechBazaar.Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TechBazaar.Ef.Repository
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        private readonly EContext eContext;

        public BaseRepository(EContext eContext)
        {
            this.eContext = eContext;
        }

        public void Add(T entity)
        {
            eContext.Set<T>().Add(entity);
        }

        public async void AddAsync(T entity)
        {
            await eContext.Set<T>().AddAsync(entity);

        }

        public void Delete(T entity)
        {
            eContext.Set<T>().Remove(entity);
        }

        public bool Exists(int id)
        {
            return eContext.Set<T>().Find(id) is not null ? true : false;
        }

        public T Find(int id)
        {
            return eContext.Set<T>().Find(id);
        }

        public async Task<T> FindAsync(int id)
        {
            return await eContext.Set<T>().FindAsync(id);
        }

        public T FirstOrDefault(Expression<Func<T, bool>> selector)
        {
            return eContext.Set<T>().FirstOrDefault(selector);
        }
        public async Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> selector)
        {
            return await eContext.Set<T>().FirstOrDefaultAsync(selector);
        }

        public IEnumerable<T> GetAll()
        {
            return eContext.Set<T>().ToList();
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> match)
        {
            return eContext.Set<T>().Where(match).ToList();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await eContext.Set<T>().ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match)
        {
            return await eContext.Set<T>().Where(match).ToListAsync();
        }

        public IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties)
        {
            var query = eContext.Set<T>().AsQueryable();
            if (includeProperties != null)
            {
                foreach (var item in includeProperties)
                {
                    query = query.Include(item);
                }
            }
            return query;
        }
        public async Task<List<T>> ToListAsync(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public IEnumerable<Result> Select<Result>(Expression<Func<T, Result>> selector)
        {
            return eContext.Set<T>().Select(selector).ToList();
        }

        public void Update(T entity)
        {
            eContext.Set<T>().Update(entity);
        }
    }
}
