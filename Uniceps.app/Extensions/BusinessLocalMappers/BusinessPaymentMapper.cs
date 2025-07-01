using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessPaymentDtos;
using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Extensions.BusinessLocalMappers
{
    public class BusinessPaymentMapper : IMapperExtension<BusinessPaymentModel, BusinessPaymentDto, BusinessPaymentCreationDto>
    {
        public BusinessPaymentModel FromCreationDto(BusinessPaymentCreationDto data)
        {
            BusinessPaymentModel businessPaymentModel = new BusinessPaymentModel();
            businessPaymentModel. BusinessSubscriptionNID =data.BusinessSubscriptionId;
            businessPaymentModel.Amount = data.Amount;
            businessPaymentModel.Description = data.Description;
            businessPaymentModel.IssueDate = data.IssueDate;
            return businessPaymentModel;
        }

        public BusinessPaymentDto ToDto(BusinessPaymentModel data)
        {
            BusinessPaymentDto businessPaymentDto=new BusinessPaymentDto();
            businessPaymentDto.Id = data.NID;
            businessPaymentDto.BusinessSubscriptionId = data.NID;
            businessPaymentDto.Amount = data.Amount;
            businessPaymentDto.Description = data.Description;
            businessPaymentDto.IssueDate = data.IssueDate;
            return businessPaymentDto;
        }
    }
}
