using System.Text;

namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class AnalysisVisitor : IVisitor
    {
        private float _totalCpu = 0;
        private int _cpuCount = 0;
        
        private float _minRam = float.MaxValue;

        public void VisitCpu(CpuMetric cpu)
        {
            _totalCpu += cpu.LoadPercent;
            _cpuCount++;
        }

        public void VisitRam(RamMetric ram)
        {
            if (ram.FreeMemoryMb < _minRam)
                _minRam = ram.FreeMemoryMb;
        }

        public string GetStats()
        {
            float avgCpu = _cpuCount > 0 ? _totalCpu / _cpuCount : 0;
            return $"Аналіз завершено:\n - Середнє CPU: {avgCpu:F2}%\n - Мінімум RAM: {_minRam} MB";
        }
    }
}