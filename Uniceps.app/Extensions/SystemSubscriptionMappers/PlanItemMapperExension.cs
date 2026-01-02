using Stripe;
using System;
using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Extensions.SystemSubscriptionMappers
{
    public class PlanItemMapperExension : IMapperExtension<PlanItem, PlanItemDto, PlanItemCreationDto>
    {
        public PlanItem FromCreationDto(PlanItemCreationDto data)
        {
            PlanItem planItem = new PlanItem();
            planItem.IsFree = data.IsFree;
            planItem.Price = data.Price;
            planItem.DaysCount = data.DaysCount;
            planItem.DurationString = data.DurationString;
            planItem.PlanNID = Guid.Parse(data.PlanId!); 

            return planItem;
        }

        public PlanItemDto ToDto(PlanItem data)
        {
            PlanItemDto planItemDto = new PlanItemDto();
            planItemDto.Id = data.Id;
            planItemDto.PlanId = data.PlanNID.ToString();
            planItemDto.IsFree = data.IsFree;
            planItemDto.Price = data.Price;
            planItemDto.DaysCount = data.DaysCount;
            planItemDto.DurationString = data.DurationString;
            return planItemDto;
        }
    }
}
