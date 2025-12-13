using System.Collections.Generic;
using System.Text;
using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Bridge
{
    public class HtmlRenderer : IReportRenderer
    {
        public string RenderReport(string title, List<ResourceLog> logs, string summary = "")
        {
            var sb = new StringBuilder();
            sb.AppendLine("<html><body>");
            sb.AppendLine($"<h1>{title}</h1>");

            if (!string.IsNullOrEmpty(summary))
            {
                sb.AppendLine("<div style='border: 2px solid #007ACC; padding: 10px; background-color: #e6f7ff; margin-bottom: 20px;'>");
                sb.AppendLine("<h3>📊 Аналітика (Visitor)</h3>");
                sb.AppendLine($"<p>{summary.Replace("\n", "<br>")}</p>");
                sb.AppendLine("</div>");
            }

            sb.AppendLine("<table border='1' cellpadding='5' cellspacing='0'>");
            sb.AppendLine("<tr style='background-color: #f2f2f2;'><th>Time</th><th>CPU Load</th><th>RAM Usage</th></tr>");

            foreach (var log in logs)
            {
                string color = log.CpuLoad > 80 ? "color:red; font-weight:bold;" : "";
                sb.AppendLine("<tr>");
                sb.AppendLine($"<td>{log.CreatedAt.ToShortTimeString()}</td>");
                sb.AppendLine($"<td style='{color}'>{log.CpuLoad}%</td>");
                sb.AppendLine($"<td>{log.RamUsage} MB</td>");
                sb.AppendLine("</tr>");
            }

            sb.AppendLine("</table>");
            sb.AppendLine($"<p><i>Generated at {System.DateTime.Now}</i></p>");
            sb.AppendLine("</body></html>");
            return sb.ToString();
        }
    }
}