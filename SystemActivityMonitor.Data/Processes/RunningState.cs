using System;

namespace SystemActivityMonitor.Data.Processes
{
    public class RunningState : IProcessState
    {
        private Random _rnd = new Random();

        public float CalculateCpuUsage(int baseLoad)
        {
            return Math.Max(0.1f, baseLoad + _rnd.Next(-2, 5));
        }

        public void Run(VirtualProcess process)
        {
        }

        public string GetStatusName() => "Running";
    }
}