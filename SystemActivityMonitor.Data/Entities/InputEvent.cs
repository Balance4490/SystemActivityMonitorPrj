using System;

namespace SystemActivityMonitor.Data.Entities
{
    public class InputEvent : BaseEntity
    {
        public Guid SessionId { get; set; }
        public string EventType { get; set; } 
        public string Details { get; set; }   
        
        public Session Session { get; set; }
    }
}