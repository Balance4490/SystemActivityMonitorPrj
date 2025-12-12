using System;

namespace SystemActivityMonitor.Data.Patterns.AbstractFactory
{

    public class CriticalCpuSensor : ICpuSensor
    {
        public int GetCpuLoad() => new Random().Next(90, 100); 
    }

    public class CriticalRamSensor : IRamSensor
    {
        public int GetFreeRam() => new Random().Next(50, 500); 
    }

    public class CriticalFactory : IMonitorFactory
    {
        public ICpuSensor CreateCpuSensor() => new CriticalCpuSensor();
        public IRamSensor CreateRamSensor() => new CriticalRamSensor();
    }
}