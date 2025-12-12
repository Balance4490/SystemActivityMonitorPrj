using System;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Repositories
{
    public interface IUserRepository : IRepository<User, Guid> { }

    public class UserRepository : RepositoryBase<User, Guid>, IUserRepository
    {
        public UserRepository(MonitorDbContext context) : base(context)
        {
        }
    }
}