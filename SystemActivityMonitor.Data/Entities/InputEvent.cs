using System;
using System.ComponentModel.DataAnnotations; 

namespace SystemActivityMonitor.Data.Entities
{
    public class InputEvent
    {
        public int Id { get; set; }
        public Guid SessionId { get; set; }
        public string EventType { get; set; } 
        public string Details { get; set; }   
        public DateTime CreatedAt { get; set; }
    }
}