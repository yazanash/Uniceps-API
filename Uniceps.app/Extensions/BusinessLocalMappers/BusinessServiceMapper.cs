using Uniceps.app.DTOs.BusinessLocalDtos.BusinessServicesDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.app.Extensions.BusinessLocalMappers
{
    public class BusinessServiceMapper : IMapperExtension<BusinessServiceModel, BusinessServiceDto, BusinessServiceCreationDto>
    {
        public BusinessServiceModel FromCreationDto(BusinessServiceCreationDto data)
        {
            BusinessServiceModel businessServiceModel = new BusinessServiceModel();
            businessServiceModel.Name = data.Name;
            businessServiceModel.Price = data.Price;
            businessServiceModel.IsActive = data.IsActive;
            businessServiceModel.DurationType = data.DurationType;
            businessServiceModel.Duration = data.Duration;
            businessServiceModel.SessionCount = data.SessionCount;
            businessServiceModel.TrainerId = data.TrainerId;
            return businessServiceModel;
        }

        public BusinessServiceDto ToDto(BusinessServiceModel data)
        {
            BusinessServiceDto businessServiceDto = new BusinessServiceDto();
            businessServiceDto.Id = data.NID;
            businessServiceDto.Name = data.Name;
            businessServiceDto.Price = data.Price;
            businessServiceDto.IsActive = data.IsActive;
            businessServiceDto.DurationType = data.DurationType;
            businessServiceDto.Duration = data.Duration;
            businessServiceDto.SessionCount = data.SessionCount;
            businessServiceDto.BusinessId = data.BusinessId;
            businessServiceDto.TrainerId = data.TrainerId;
            return businessServiceDto;
        }
    }
}
