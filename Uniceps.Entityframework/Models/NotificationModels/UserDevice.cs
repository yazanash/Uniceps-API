using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.NotificationModels
{
    public class UserDevice: EntityBase
    {
        public string? UserId { get; set; }
        public string DeviceToken { get; set; } = null!; 
        public string DeviceId { get; set; } = null!; 
        public string Platform { get; set; } = null!; 
        public string AppVersion { get; set; } = null!;
        public string DeviceModel { get; set; } = null!;
        public string OsVersion { get; set; } = null!;
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
