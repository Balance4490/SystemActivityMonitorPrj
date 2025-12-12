using System;
using System.Linq;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class SystemController
    {
        public void GenerateTestData()
        {
            using (var db = new MonitorDbContext())
            {
                var admin = db.Users.FirstOrDefault(u => u.Username == "admin");
                if (admin == null) return;

                var session = new Session 
                { 
                    UserId = admin.Id, 
                    MachineName = "COMMAND-PC", 
                    OSVersion = "Windows 11 Pro" 
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                var rnd = new Random();
                for (int i = 0; i < 5; i++)
                {
                    db.ResourceLogs.Add(new ResourceLog
                    {
                        SessionId = session.Id,
                        CpuLoad = rnd.Next(10, 90),
                        RamUsage = rnd.Next(4000, 16000),
                        CreatedAt = DateTime.UtcNow.AddSeconds(i * 5)
                    });
                }
                db.SaveChanges();
            }
        }

        public void ClearAllData()
        {
            using (var db = new MonitorDbContext())
            {
                db.ResourceLogs.RemoveRange(db.ResourceLogs);
                db.Sessions.RemoveRange(db.Sessions);
                db.SaveChanges();
            }
        }
    }
}