namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public interface IMetricElement
    {
        void Accept(IVisitor visitor);
    }
}