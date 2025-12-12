using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SystemActivityMonitor.Data.Repositories
{
    public abstract class RepositoryBase<T, TKey> : IRepository<T, TKey> where T : class
    {
        protected readonly MonitorDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public RepositoryBase(MonitorDbContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public async Task<T> GetById(TKey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public void Add(T entity)
        {
            _dbSet.Add(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}