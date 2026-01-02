using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class CashRequestDto
    {
        public int Id { get; set; }
        public string Email { get; set; } = null!;
        public string PaymentGateway { get; set; } = "";
        public string? Subscription { get; set; }
        public string? SubscriptionPrice { get; set; }
        public string TransferCode { get; set; } = null!;
        public decimal? Amount { get; set; }
        public string? ImageUrl { get; set; }
        public string Status { get; set; } = "";
        public string CreatedAt { get; set; } = "";
    }
}
