namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public interface IVisitor
    {
        void VisitCpu(CpuMetric cpu);
        void VisitRam(RamMetric ram);
    }
}