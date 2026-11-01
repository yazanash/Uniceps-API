using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanItemCreationDto
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string? DurationString { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int ProductId { get; set; }
        public PlanItem ToModel()
        {
            PlanItem planItem = new PlanItem()
            {
                IsFree = IsFree,
                Price = Price,
                DaysCount = DaysCount,
                DurationString = DurationString,
                Name = Name,
                ProductId = ProductId
            };

            return planItem;
        }
    }
}
