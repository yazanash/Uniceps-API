using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class PlanItemDto
    {
        public PlanItemDto (PlanItem data)
        {

            Id = data.Id;
            IsFree = data.IsFree;
            Price = data.Price;
            DaysCount = data.DaysCount;
            DurationString = data.DurationString;
            Name = data.Name;
            ProductId = data.ProductId;
          
        }
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string? DurationString { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
