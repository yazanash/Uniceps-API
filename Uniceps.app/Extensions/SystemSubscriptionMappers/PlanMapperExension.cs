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
            plan.Price = data.Price;
            plan.DurationInDays = data.Days;
            plan.IsFree = data.IsFree;
            plan.TargetUserType = (PlanTarget) data.TargetUserType;
            return plan;
        }

        public PlanDto ToDto(PlanModel data)
        {
            PlanDto planDto = new PlanDto();
            planDto.Id = data.Id;
            planDto.Name = data.Name;
            planDto.Price = data.Price;
            planDto.Days = data.DurationInDays;
            planDto.TargetUserType = (int)data.TargetUserType;
            planDto.IsFree = data.IsFree;
            return planDto;
        }
    }
}
