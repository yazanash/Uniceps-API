using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessSubscriptionsDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Extensions.BusinessLocalMappers
{
    public class BusinessSubscriptionMapper : IMapperExtension<BusinessSubscriptionModel, BusinessSubscriptionDto, BusinessSubscriptionCreationDto>
    {
        private readonly IMapperExtension<BusinessPaymentModel, BusinessPaymentDto, BusinessPaymentCreationDto> _mapperExtension ;

        public BusinessSubscriptionMapper(IMapperExtension<BusinessPaymentModel, BusinessPaymentDto, BusinessPaymentCreationDto> mapperExtension)
        {
            _mapperExtension = mapperExtension;
        }

        public BusinessSubscriptionModel FromCreationDto(BusinessSubscriptionCreationDto data)
        {
           BusinessSubscriptionModel model = new BusinessSubscriptionModel();
            model. ServiceNID=data.ServiceId;
            model.PlayerId = data.ServiceId;
            model.RollDate = data.RollDate;
            model.Price = data.Price;
            model.OfferValue = data.OfferValue;
            model.OfferDes = data.OfferDes;
            model.PriceAfterOffer = data.PriceAfterOffer;
            model.SessionCount = data.SessionCount;
            model.IsStopped = data.IsStopped;
            model.PaidValue = data.PaidValue;
            model.RestValue = data.RestValue;
            model.EndDate = data.EndDate;
            model.LastPaid = data.LastPaid;
            return model;
        }

        public BusinessSubscriptionDto ToDto(BusinessSubscriptionModel data)
        {
            BusinessSubscriptionDto businessSubscriptionDto = new BusinessSubscriptionDto();
            businessSubscriptionDto.Id = data.NID;
            businessSubscriptionDto.ServiceId = data.NID;
            businessSubscriptionDto.PlayerId = data.PlayerId.ToString();
            businessSubscriptionDto.RollDate = data.RollDate;
            businessSubscriptionDto.Price = data.Price;
            businessSubscriptionDto.OfferValue = data.OfferValue;
            businessSubscriptionDto.OfferDes = data.OfferDes;
            businessSubscriptionDto.PriceAfterOffer = data.PriceAfterOffer;
            businessSubscriptionDto.SessionCount = data.SessionCount;
            businessSubscriptionDto.IsStopped = data.IsStopped;
            businessSubscriptionDto.PaidValue = data.PaidValue;
            businessSubscriptionDto.RestValue = data.RestValue;
            businessSubscriptionDto.EndDate = data.EndDate;
            businessSubscriptionDto.LastPaid = data.LastPaid;
            businessSubscriptionDto.BusinessPayments.AddRange(data.BusinessPaymentModels.Select(x => _mapperExtension.ToDto(x)));
            return businessSubscriptionDto;
        }
    }
}
