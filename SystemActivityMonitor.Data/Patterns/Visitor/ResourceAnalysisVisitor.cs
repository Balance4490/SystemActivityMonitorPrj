using System.Text;
using SystemActivityMonitor.Data.Processes;

namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class ResourceAnalysisVisitor : IProcessVisitor
    {
        private int _browserCount = 0;
        private float _browserRam = 0;
        
        private int _devToolsCount = 0;
        private float _devToolsCpu = 0;

        private int _criticalProcessCount = 0;

        public void Visit(VirtualProcess process)
        {
            if (process.Name.Contains("Chrome") || process.Name.Contains("Edge"))
            {
                _browserCount++;
                _browserRam += process.GetCurrentRamUsage();
            }

            if (process.Name.Contains("Visual Studio") || process.Name.Contains("Rider"))
            {
                _devToolsCount++;
                _devToolsCpu += process.GetCurrentCpuUsage();
            }

            if (process.GetStatus() == "Not Responding")
            {
                _criticalProcessCount++;
            }
        }

        public string GetReport()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== ЗВІТ АНАЛІЗАТОРА (VISITOR) ===");
            sb.AppendLine($"Всього браузерів: {_browserCount}");
            sb.AppendLine($"RAM, яку їдять браузери: {_browserRam:F1} MB");
            sb.AppendLine("----------------------------------");
            sb.AppendLine($"Всього IDE (DevTools): {_devToolsCount}");
            sb.AppendLine($"Навантаження CPU від IDE: {_devToolsCpu:F1}%");
            sb.AppendLine("----------------------------------");
            sb.AppendLine($"ЗАВИСЛИХ ПРОЦЕСІВ: {_criticalProcessCount}");
            
            if (_criticalProcessCount > 0)
                sb.AppendLine(">>> РЕКОМЕНДАЦІЯ: Негайно вбийте завислі процеси!");
            else
                sb.AppendLine("Система працює стабільно.");

            return sb.ToString();
        }
    }
}