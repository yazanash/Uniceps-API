using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.Extensions.ProfileMappers
{
    public class BusinessProfileMapper : IMapperExtension<BusinessProfile, BusinessProfileDto, BusinessProfileCreationDto>
    {
        public BusinessProfile FromCreationDto(BusinessProfileCreationDto data)
        {
            BusinessProfile businessProfile = new BusinessProfile();
            businessProfile.BusinessName = data.B_Name;
            businessProfile.OwnerName = data.OwnerName;
            businessProfile.Phone1 = data.Phone1;
            businessProfile.Phone2 = data.Phone2;
            businessProfile.BusinessType = (BusinessType) data.BusinessType;
            return businessProfile;
        }

        public BusinessProfileDto ToDto(BusinessProfile data)
        {
            BusinessProfileDto businessProfile = new BusinessProfileDto();
            businessProfile.Id = data.Id;
            businessProfile.BusinessName = data.BusinessName;
            businessProfile.OwnerName = data.BusinessName;
            businessProfile.Phone1 = data.BusinessName;
            businessProfile.Phone2 = data.BusinessName;
            businessProfile.PictureUrl = data.BusinessName;
            businessProfile.BusinessType =(int) data.BusinessType;
            return businessProfile;
        }
    }
}
