namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public class RamMetric : IMetricElement
    {
        public float FreeMemoryMb { get; set; }

        public RamMetric(float freeMem)
        {
            FreeMemoryMb = freeMem;
        }

        public void Accept(IVisitor visitor)
        {
            visitor.VisitRam(this);
        }
    }
}