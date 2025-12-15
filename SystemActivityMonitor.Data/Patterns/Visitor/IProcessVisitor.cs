using SystemActivityMonitor.Data.Processes;

namespace SystemActivityMonitor.Data.Patterns.Visitor
{
    public interface IProcessVisitor
    {
        void Visit(VirtualProcess process);
    }
}