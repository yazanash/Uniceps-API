using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.SystemSubscriptionModels
{
    public class CashPaymentRequest
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public string Email { get; set; } = null!;
        public int PaymentGatewayId { get; set; }
        public int PlanItemId { get; set; }
        public string TransferCode { get; set; } = null!;
        public CashRequestStatus Status { get; set; } = CashRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? UserId { get; set; }
        public int? CouponId { get; set; }
    }
    public enum CashRequestStatus
    {
        Pending=0,
        Accepted=1,
        Declined=2
    }
}
