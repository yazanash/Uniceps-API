using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class ChangeCashRequestStatusDto
    {
        public int Id {  get; set; }
        public CashRequestStatus CashRequestStatus { get; set; }
    }
}
