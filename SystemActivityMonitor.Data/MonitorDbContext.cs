using Microsoft.EntityFrameworkCore;
using SystemActivityMonitor.Data.Entities;
using System.Security.Cryptography;
using System.Text;

namespace SystemActivityMonitor.Data
{
    public class MonitorDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<ResourceLog> ResourceLogs { get; set; }
        public DbSet<InputEvent> InputEvents { get; set; }

        private readonly string _dbPath = "system_monitor.db";

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User 
                { 
                    Id = Guid.Parse("11111111-1111-1111-1111-111111111111"),
                    Username = "admin", 
                    PasswordHash = HashPassword("admin123"), 
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                },
                new User 
                { 
                    Id = Guid.Parse("22222222-2222-2222-2222-222222222222"),
                    Username = "user", 
                    PasswordHash = HashPassword("user123"), 
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                }
            );
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var builder = new StringBuilder();
                foreach (var b in bytes)
                {
                    builder.Append(b.ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}