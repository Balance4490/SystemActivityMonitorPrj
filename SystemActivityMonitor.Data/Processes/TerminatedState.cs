namespace SystemActivityMonitor.Data.Processes
{
    public class TerminatedState : IProcessState
    {
        public float CalculateCpuUsage(int baseLoad)
        {
            return 0; 
        }

        public void Run(VirtualProcess process)
        {
        }

        public string GetStatusName() => "Terminated";
    }
}