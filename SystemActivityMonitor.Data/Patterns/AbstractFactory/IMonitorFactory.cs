namespace SystemActivityMonitor.Data.Patterns.AbstractFactory
{
    public interface IMonitorFactory
    {
        ICpuSensor CreateCpuSensor();
        IRamSensor CreateRamSensor();
    }
}