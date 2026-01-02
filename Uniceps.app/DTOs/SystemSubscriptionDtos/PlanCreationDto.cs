using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanCreationDto
    {
        public string? Name { get; set; }
        public int ProductId { get; set; }
    }
}
