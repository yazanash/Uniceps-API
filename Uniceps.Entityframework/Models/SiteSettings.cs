using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public class SiteSettings
    {
        public int Id { get; set; }
        public string ContactEmail { get; set; } = string.Empty;
        public string ContactPhone { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? FacebookUrl { get; set; }
        public string? WhatsAppNumber { get; set; }
        public string? TelegramUser { get; set; }
        public bool IsMaintenanceMode { get; set; } = false; 
    }
}
