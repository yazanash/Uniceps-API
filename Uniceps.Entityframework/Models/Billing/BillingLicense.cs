using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Billing
{
    public class BillingLicense
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int ProductId { get; set; }
        public string CustomerName { get; set; } = "";
        public LicenseType Type { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int MaxDevices { get; set; } = 1;
        public string ServerSignature { get; set; } = "";
    }
    public enum LicenseType
    {
        Subscription,
        LifeTime
    }
}
