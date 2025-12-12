using System;
using System.Linq;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.AbstractFactory;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class SystemController
    {
        public void ClearAllData()
        {
            using (var db = new MonitorDbContext())
            {
                db.ResourceLogs.RemoveRange(db.ResourceLogs);
                db.Sessions.RemoveRange(db.Sessions);
                db.SaveChanges();
            }
        }

        public void GenerateDataWithFactory(IMonitorFactory factory)
        {
            ICpuSensor cpuSensor = factory.CreateCpuSensor();
            IRamSensor ramSensor = factory.CreateRamSensor();

            using (var db = new MonitorDbContext())
            {
                var admin = db.Users.FirstOrDefault(u => u.Username == "admin");
                if (admin == null) return;

                var session = new Session
                {
                    UserId = admin.Id,
                    MachineName = "FACTORY-PC",
                    OSVersion = "Windows 11"
                };
                db.Sessions.Add(session);
                db.SaveChanges();

                for (int i = 0; i < 10; i++)
                {
                    db.ResourceLogs.Add(new ResourceLog
                    {
                        SessionId = session.Id,
                        CpuLoad = cpuSensor.GetCpuLoad(),
                        RamUsage = ramSensor.GetFreeRam(),
                        CreatedAt = DateTime.UtcNow.AddSeconds(i * 2)
                    });
                }
                db.SaveChanges();
            }
        }
    }
}