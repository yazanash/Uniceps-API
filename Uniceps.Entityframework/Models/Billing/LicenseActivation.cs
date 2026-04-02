using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Billing
{
    public class LicenseActivation
    {
        public Guid Id { get; set; }
        public Guid LicenseId { get; set; }
        public string MachineId { get; set; } = string.Empty;
        public string ActivationToken { get; set; } =string.Empty; 
        public DateTime ActivatedAt { get; set; }
        public DateTime? ExpiredAt { get; set; }
        public bool IsUsed { get; set; } = false;
    }
}
