using System;
using System.Collections.Generic;

namespace SystemActivityMonitor.Data.Entities
{
    public class Session : BaseEntity
    {
        public Guid UserId { get; set; }
        public DateTime? EndedAt { get; set; }
        public string MachineName { get; set; }
        public string OSVersion { get; set; }
        public User User { get; set; }
        public ICollection<ResourceLog> ResourceLogs { get; set; } = new List<ResourceLog>();
    }
}