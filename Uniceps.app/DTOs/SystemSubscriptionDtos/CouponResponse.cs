using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class CouponResponse
    {
        private SubscriptionCoupon SubscriptionCoupon;

        public CouponResponse(SubscriptionCoupon subscriptionCoupon)
        {
            SubscriptionCoupon = subscriptionCoupon;
        }
        public int Id => SubscriptionCoupon.Id;
        public string Code => SubscriptionCoupon.Code;
        public DateTime CreatedAt => SubscriptionCoupon.CreatedAt;
        public DateTime ExpirationAt => SubscriptionCoupon.ExpirationAt;
        public string? DescriptionMessage => SubscriptionCoupon.DescriptionMessage;
        public double CouponValue => SubscriptionCoupon.CouponValue;
    }
}
