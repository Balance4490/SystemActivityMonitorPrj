namespace SystemActivityMonitor.Data.Patterns.AbstractFactory
{
    public interface ICpuSensor
    {
        int GetCpuLoad();
    }

    public interface IRamSensor
    {
        int GetFreeRam();
    }
}