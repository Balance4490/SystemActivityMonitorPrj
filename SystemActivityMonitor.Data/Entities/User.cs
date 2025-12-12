using System.Collections.Generic;

namespace SystemActivityMonitor.Data.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } 
        public string SettingsJson { get; set; } 
        
        public ICollection<Session> Sessions { get; set; } = new List<Session>();
    }
}