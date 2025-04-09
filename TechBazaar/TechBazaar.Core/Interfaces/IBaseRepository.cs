using System.Linq.Expressions;

namespace TechBazaar.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        // Synchronous methods
        T Find(int id);
        IEnumerable<T> GetAll();
        IEnumerable<T> GetAll(Expression<Func<T, bool>> match);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
        IQueryable<T> Include(params Expression<Func<T, object>>[] includeProperties);
        bool Exists(int id);
        IEnumerable<Result> Select<Result>(Expression<Func<T, Result>> selector);
        T FirstOrDefault(Expression<Func<T, bool>> selector);
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> selector);
        // Asynchronous methods

        Task<T> FindAsync(int id);
        Task<IEnumerable<T>> GetAllAsync();
        Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> match);
        void AddAsync(T entity);
        Task<List<T>> ToListAsync(IQueryable<T> query);

    }
}
