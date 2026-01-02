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
        public string DeviceToken { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string Platform { get; set; } = "";
        public string AppVersion { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string OsVersion { get; set; } = "";
        public string NotifyToken { get; set; } = "";
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
        public DateTime LastSeen { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; } = true;
    }
}
