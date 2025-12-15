namespace SystemActivityMonitor.Data.Processes
{
    public class NotRespondingState : IProcessState
    {
        public float CalculateCpuUsage(int baseLoad)
        {

            return 95.0f; 
        }

        public void Run(VirtualProcess process)
        {
        }

        public string GetStatusName() => "Not Responding";
    }
}