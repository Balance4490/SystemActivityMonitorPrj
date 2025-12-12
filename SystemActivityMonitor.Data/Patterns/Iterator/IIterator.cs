using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Iterator
{
    public interface IIterator
    {
        void First();              
        void Next();               
        bool IsDone();            
        ResourceLog CurrentItem();  
    }
}