using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string UserId { get; set; } = "";
        public string Title { get; set; } = "";
        public string Body { get; set; } = "";
        public DateTime ScheduledTime  { get; set; } =DateTime.UtcNow;
        public int RetryCount { get; set; }
    }
}
