using System.Text;

namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class XmlExportVisitor : IVisitor
    {
        private StringBuilder _sb = new StringBuilder();

        public string GetXml() => $"<Report>\n{_sb}</Report>";

        public void VisitCpu(CpuMetric cpu)
        {
            _sb.AppendLine($"  <CpuMetric load='{cpu.LoadPercent}%' time='{cpu.Time}' />");
        }

        public void VisitRam(RamMetric ram)
        {
            _sb.AppendLine($"  <RamMetric free='{ram.FreeMemoryMb}MB' />");
        }
    }
}