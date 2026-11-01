using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.DTOs.ProfileDtos
{
    public class NormalProfileCreationDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? PictureUrl { get; set; }
        public NormalProfile ToModel()
        {
            NormalProfile normalProfile = new NormalProfile();
            normalProfile.Name = Name;
            normalProfile.Phone = Phone;
            normalProfile.Address = Address;
            normalProfile.OwnerName = OwnerName;
            normalProfile.DateOfBirth = DateOfBirth;
            normalProfile.Gender = (GenderType)Gender;
            return normalProfile;
        }
    }
}
