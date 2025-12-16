using System;
using System.Collections.Generic;
using System.Linq;
using SystemActivityMonitor.Data.Entities;
using SystemActivityMonitor.Data.Patterns.AbstractFactory;
using SystemActivityMonitor.Data.Patterns.Observer;
using SystemActivityMonitor.Data.Processes;
using SystemActivityMonitor.Data.Patterns.Visitor;

namespace SystemActivityMonitor.Data.Patterns.Command
{
    public class SystemController : ISubject
    {
        private List<VirtualProcess> _activeProcesses = new List<VirtualProcess>();
        private List<IObserver> _observers = new List<IObserver>();
        private readonly string[] _protectedProcesses = { "System", "Registry", "Antimalware Service" };

        public float CurrentTotalCpu { get; private set; }
        public float CurrentTotalRam { get; private set; }

        public SystemController()
        {
            _activeProcesses = new List<VirtualProcess>();

            _activeProcesses.Add(new VirtualProcess("System", 1, 150));
            _activeProcesses.Add(new VirtualProcess("Registry", 0, 50));
            _activeProcesses.Add(new VirtualProcess("Antimalware Service", 2, 200));

            RecalculateLoad();
        }

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
                if (_protectedProcesses.Contains(process.Name))
                {
                    throw new InvalidOperationException($"Доступ заборонено! Процес '{process.Name}' є критичним для системи.");
                }

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

        public void IncreaseProcessLoad(Guid id)
        {
            var proc = _activeProcesses.FirstOrDefault(p => p.Id == id);
            if (proc != null)
            {
                proc.IncreaseLoad();
                RecalculateLoad();
                Notify();
            }
        }

        public void DecreaseProcessLoad(Guid id)
        {
            var proc = _activeProcesses.FirstOrDefault(p => p.Id == id);
            proc?.DecreaseLoad();
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
                if (_activeProcesses.Count > 0)
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
                                ActiveWindow = _activeProcesses.Last().Name,
                                CreatedAt = DateTime.UtcNow,
                                IsSystemIdle = false
                            });

                            var rnd = new Random();
                            if (rnd.Next(0, 10) < 3)
                            {
                                bool isMouse = rnd.Next(0, 2) == 0;
                                db.InputEvents.Add(new InputEvent
                                {
                                    SessionId = session.Id,
                                    EventType = isMouse ? "Mouse" : "Keyboard",
                                    Details = isMouse ? "Click" : "Key Press",
                                    CreatedAt = DateTime.UtcNow
                                });
                            }

                            db.SaveChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Помилка БД: " + ex.Message);
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

        public void ApplyVisitor(IProcessVisitor visitor)
        {
            var currentProcesses = _activeProcesses.ToList();
            foreach (var process in currentProcesses)
            {
                process.Accept(visitor);
            }
        }

        public List<VirtualProcess> GetProcesses()
        {
            return _activeProcesses;
        }
    }
}