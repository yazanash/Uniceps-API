using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.ProfileDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.ProfileMappers
{
    public class NormalProfileMapper : IMapperExtension<NormalProfile, NormalProfileDto, NormalProfileCreationDto>
    {
        public NormalProfile FromCreationDto(NormalProfileCreationDto data)
        {
          NormalProfile normalProfile = new NormalProfile();
            normalProfile.Name = data.Name;
            normalProfile.Phone = data.Phone;
            normalProfile.Address = data.Address;
            normalProfile.OwnerName = data.OwnerName;
            normalProfile.DateOfBirth = data.DateOfBirth;
            normalProfile.Gender = (GenderType)data.Gender;
            return normalProfile;
        }

        public NormalProfileDto ToDto(NormalProfile data)
        {
            NormalProfileDto normalProfile = new NormalProfileDto();
            normalProfile.Id = data.NID;
            normalProfile.Name = data.Name;
            normalProfile.Phone = data.Phone;
            normalProfile.OwnerName = data.OwnerName;
            normalProfile.Address = data.Address;
            normalProfile.DateOfBirth = data.DateOfBirth;
            normalProfile.Gender =(int) data.Gender;
            normalProfile.PictureUrl = data.PictureUrl;
            return normalProfile;
    }
    }
}
