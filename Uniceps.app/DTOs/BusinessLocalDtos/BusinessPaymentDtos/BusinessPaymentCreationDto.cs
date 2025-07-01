using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos
{
    public class BusinessPaymentCreationDto
    {
        public Guid BusinessSubscriptionId { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
