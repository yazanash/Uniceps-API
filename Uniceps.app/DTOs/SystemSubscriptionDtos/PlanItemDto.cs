using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanItemDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string? DurationString { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
        public string? PlanId { get; set; }
    }
}
