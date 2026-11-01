using System.ComponentModel.DataAnnotations;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class CouponRequest
    {
        public string Code { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationAt { get; set; } = DateTime.UtcNow;
        public string? DescriptionMessage { get; set; }
        public double CouponValue { get; set; }

        internal SubscriptionCoupon ToModel()
        {
            return new SubscriptionCoupon()
            {
                Code = Code,
                CreatedAt = CreatedAt,
                ExpirationAt = ExpirationAt,
                CouponValue = CouponValue,
                DescriptionMessage = DescriptionMessage
            };
        }
    }
}
