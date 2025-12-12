using SystemActivityMonitor.Data.Entities;

namespace SystemActivityMonitor.Data.Patterns.Iterator
{
    public class LogIterator : IIterator
    {
        private readonly LogCollection _aggregate;
        private int _current = 0;

        public LogIterator(LogCollection aggregate)
        {
            _aggregate = aggregate;
        }

        public void First()
        {
            _current = 0;
        }

        public void Next()
        {
            _current++;
        }

        public bool IsDone()
        {
            return _current >= _aggregate.Count;
        }

        public ResourceLog CurrentItem()
        {
            if (IsDone())
                return null;
                
            return _aggregate[_current] as ResourceLog;
        }
    }
}