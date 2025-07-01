using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int Days { get; set; }
        public int TargetUserType { get; set; }
        public bool IsFree { get; set; }
    }
}
