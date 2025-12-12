namespace SystemActivityMonitor.Data.Patterns.Iterator
{
    public interface IAggregate
    {
        IIterator CreateIterator();
        int Count { get; }
        object this[int index] { get; set; } 
    }
}