using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models
{
    public class TelegramUserState
    {
        public int Id { get; set; }
        public long ChatId { get; set; }        // Telegram chat id
        public BotStep Step { get; set; } = BotStep.Start;
        public string? Email { get; set; }
        public string? TransferCode { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public int PaymentGatewayId { get; set; }
        public int PlanId { get; set; }
        public string UserId { get; set; } = string.Empty;
        public string? Coupon { get; set; }
        public int? CouponId { get; set; }
        public double Discount { get; set; }
    }
    public enum BotStep
    {
        Start,
        WaitingEmail,
        WaitingPlan,
        ChoosingPlan,
        WaitingCoupon,
        ChoosingGateway,
        WaitingGateway,
        WaitingTransferCode,
        Done,
        WaitingConfirmation,
        WaitingCheckoutConfirmation,
    }
    public enum BotTrigger
    {
        Start,
        EmailSubmitted,
        PlanSelected,
        CouponSubmitted,
        CouponSkipped,
        ConfirmCheckout,
        GatewaySelected,
        TransferCodeSubmitted,
        ConfirmPayment,
        Cancel,
        CheckoutCanceled
    }
}
