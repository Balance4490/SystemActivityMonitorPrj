namespace SystemActivityMonitor.Data.Patterns.Observer
{
    public interface ISubject
    {
        void Attach(IObserver observer); 
        void Detach(IObserver observer); 
        void Notify();                   
    }
}