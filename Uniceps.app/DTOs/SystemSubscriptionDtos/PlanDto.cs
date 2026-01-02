using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public int ProductId { get; set; }
        public int TargetUserType { get; set; } = 0;
        public List<PlanItemDto> PlanItems { get; set; } = new();
    }
}
