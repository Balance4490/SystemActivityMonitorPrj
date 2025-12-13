using System.Collections.Generic;
using System.Text;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public class PlainTextRenderer : IReportRenderer
    {
        public string RenderReport(string title, List<ResourceLog> logs, string summary = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine($"=== {title} ===");
            sb.AppendLine($"Згенеровано: {System.DateTime.Now}");
            
            if (!string.IsNullOrEmpty(summary))
            {
                sb.AppendLine("\n[АНАЛІТИКА СИСТЕМИ]");
                sb.AppendLine(summary);
            }

            sb.AppendLine("\n[ДЕТАЛЬНИЙ ЛОГ]");
            sb.AppendLine("--------------------------------------------------");
            foreach (var log in logs)
            {
                sb.AppendLine($"TIME: {log.CreatedAt.ToShortTimeString()} | CPU: {log.CpuLoad}% | RAM: {log.RamUsage} MB");
            }
            return sb.ToString();
        }
    }
}