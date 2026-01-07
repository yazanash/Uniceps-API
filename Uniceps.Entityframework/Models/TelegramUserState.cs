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
        public string? TransferImageUrl { get; set; }
        public string? PaymentMethod { get; set; }
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public int PaymentGatewayId { get; set; }
        public string? SubscriptionId { get; set; }
        public decimal? Amount { get; set; }
        public string? ReceiptFileId { get; set; }
    }
    public enum BotStep
    {
        Start,
        WaitingEmail,
        WaitingConfirmation,
        ShowingInvoice,
        ChoosingGateway,
        ChoosingSubscription,
        WaitingGateway,
        WaitingReceiptImage,
        Done
    }
}
