using System.Linq.Expressions;

namespace TechBazaar.Core.Interfaces
{
    public interface IBaseRepository<T> where T : class
    {
        T FindById(int id);
        Task<T> FindByIdAsync(int id);

        IEnumerable<T> FindAll();
        T SelectOne(Expression<Func<T,bool>> match);
        void Add(T item);
        void AddRange(IEnumerable<T> items);
        void Edit(T item);
        void EditRange(IEnumerable<T> items);
        void Delete(T item);
        void DeleteRange(IEnumerable<T> items);
        void SaveChanges();

    }
}
