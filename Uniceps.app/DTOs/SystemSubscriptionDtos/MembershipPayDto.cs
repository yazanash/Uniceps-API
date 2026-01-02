namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class MembershipPayDto
    {
        public bool RequirePayment { get; set; }
        public string? PaymentUrl { get; set; }
        public string? Message { get; set; }
    }
}
