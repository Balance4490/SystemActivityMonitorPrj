using System;
using System.Collections.Generic;
using System.Linq;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.AbstractFactory;
using SystemActivityMonitor.Data.Patterns.Observer;
using SystemActivityMonitor.Data.Processes;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class SystemController : ISubject
    {
        private List<VirtualProcess> _activeProcesses = new List<VirtualProcess>();
        private List<IObserver> _observers = new List<IObserver>();

        public float CurrentTotalCpu { get; private set; }
        public float CurrentTotalRam { get; private set; }

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify()
        {
            foreach (var observer in _observers)
            {
                observer.Update(CurrentTotalCpu, CurrentTotalRam, _activeProcesses);
            }
        }

        public void StartProcess(string name, int baseCpu, int ramMb)
        {
            var process = new VirtualProcess(name, baseCpu, ramMb);
            _activeProcesses.Add(process);

            RecalculateLoad();
            Notify();
        }

        public void KillProcess(Guid processId)
        {
            var process = _activeProcesses.FirstOrDefault(p => p.Id == processId);
            if (process != null)
            {
                process.SetState(new TerminatedState());
                _activeProcesses.Remove(process);

                RecalculateLoad();
                Notify();
            }
        }

        public void SetProcessState(Guid processId, IProcessState newState)
        {
            var process = _activeProcesses.FirstOrDefault(p => p.Id == processId);
            if (process != null)
            {
                process.SetState(newState);
                RecalculateLoad();
                Notify();
            }
        }

        public void UpdateSystemState()
        {
            RecalculateLoad();
            Notify();
        }

        private void RecalculateLoad()
        {
            float cpuSum = 0;
            float ramSum = 0;

            foreach (var proc in _activeProcesses)
            {
                cpuSum += proc.GetCurrentCpuUsage();
                ramSum += proc.GetCurrentRamUsage();
            }

            CurrentTotalCpu = Math.Min(100.0f, cpuSum);
            CurrentTotalRam = ramSum;

            try
            {
                using (var db = new MonitorDbContext())
                {
                    var session = db.Sessions.OrderByDescending(s => s.CreatedAt).FirstOrDefault();
                    if (session != null)
                    {
                        db.ResourceLogs.Add(new ResourceLog
                        {
                            SessionId = session.Id,
                            CpuLoad = CurrentTotalCpu,
                            RamUsage = CurrentTotalRam,
                            ActiveWindow = _activeProcesses.Count > 0 ? _activeProcesses.Last().Name : "Desktop",
                            CreatedAt = DateTime.UtcNow,
                            IsSystemIdle = _activeProcesses.Count == 0
                        });
                        db.SaveChanges();
                    }
                }
            }
            catch
            {
                // Ігноруємо помилки БД
            }
        }

        public void GenerateTestData()
        {
        }

        public void ClearAllData()
        {
            using (var db = new MonitorDbContext())
            {
                db.ResourceLogs.RemoveRange(db.ResourceLogs);
                db.Sessions.RemoveRange(db.Sessions);
                db.SaveChanges();
            }
            _activeProcesses.Clear();
            RecalculateLoad();
            Notify();
        }

        public void GenerateDataWithFactory(IMonitorFactory factory)
        {
        }
    }
}