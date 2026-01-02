using Stripe;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Extensions.SystemSubscriptionMappers
{
    public class PlanMapperExension : IMapperExtension<PlanModel, PlanDto, PlanCreationDto>
    {
        public PlanModel FromCreationDto(PlanCreationDto data)
        {
            PlanModel plan = new PlanModel();
            plan.Name = data.Name;
            plan.ProductId = data.ProductId;
            return plan;
        }

        public PlanDto ToDto(PlanModel data)
        {
            PlanDto planDto = new PlanDto();
            planDto.Id = data.NID;
            planDto.Name = data.Name;
            planDto.ProductId = data.ProductId;
            foreach (PlanItem planItem in data.PlanItems)
            {
                PlanItemDto planItemDto = new PlanItemDto();
                planItemDto.Id = planItem.Id;
                planItemDto.PlanId = planItem.PlanNID.ToString();
                planItemDto.IsFree = planItem.IsFree;
                planItemDto.Price = planItem.Price;
                planItemDto.DaysCount = planItem.DaysCount;
                planItemDto.DurationString = planItem.DurationString;
                planDto.PlanItems.Add(planItemDto);
            }
            return planDto;
        }
    }
}
