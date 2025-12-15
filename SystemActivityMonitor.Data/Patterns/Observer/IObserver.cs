using System.Collections.Generic;
using SystemActivityMonitor.Data.Processes;

namespace SystemActivityMonitor.Data.Patterns.Observer
{
    public interface IObserver
    {
        void Update(float totalCpu, float totalRam, List<VirtualProcess> processes);
    }
}