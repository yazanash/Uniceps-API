using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos
{
    public class BusinessPaymentDto
    {
        public Guid Id { get; set; }
        public Guid BusinessSubscriptionNID { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime IssueDate { get; set; }
    }
}
