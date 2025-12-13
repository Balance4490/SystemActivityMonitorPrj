namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class CpuMetric : IMetricElement
    {
        public float LoadPercent { get; set; }
        public string Time { get; set; }

        public CpuMetric(float load, string time)
        {
            LoadPercent = load;
            Time = time;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitCpu(this);
        }
    }
}