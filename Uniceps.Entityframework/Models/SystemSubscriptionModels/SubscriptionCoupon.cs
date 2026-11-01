using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.SystemSubscriptionModels
{
    public class SubscriptionCoupon
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationAt { get; set; } = DateTime.UtcNow;
        public string? DescriptionMessage { get; set; }
        public double CouponValue { get; set; }
    }
}
