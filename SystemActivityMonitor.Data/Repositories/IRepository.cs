using System.Linq;
using System.Threading.Tasks;

namespace SystemActivityMonitor.Data.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        Task<T> GetById(TKey id);
        IQueryable<T> GetAll();
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        Task SaveChangesAsync();
    }
}