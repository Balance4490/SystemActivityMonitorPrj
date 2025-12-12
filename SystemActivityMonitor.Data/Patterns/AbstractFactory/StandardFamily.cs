using System;

namespace SystemActivityMonitor.Data.Patterns.AbstractFactory
{
    public class StandardCpuSensor : ICpuSensor
    {
        public int GetCpuLoad() => new Random().Next(10, 45); 
    }

    public class StandardRamSensor : IRamSensor
    {
        public int GetFreeRam() => new Random().Next(4000, 16000); 
    }

    public class StandardFactory : IMonitorFactory
    {
        public ICpuSensor CreateCpuSensor() => new StandardCpuSensor();
        public IRamSensor CreateRamSensor() => new StandardRamSensor();
    }
}