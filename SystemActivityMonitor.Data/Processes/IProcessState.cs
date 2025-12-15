namespace SystemActivityMonitor.Data.Processes
{
    public interface IProcessState
    {
        float CalculateCpuUsage(int baseLoad); 
        void Run(VirtualProcess process);      
        string GetStatusName();                
    }
}