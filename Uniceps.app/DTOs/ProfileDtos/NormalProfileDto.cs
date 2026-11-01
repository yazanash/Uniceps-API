using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.DTOs.ProfileDtos
{
    public class NormalProfileDto
    {
        public NormalProfileDto(NormalProfile data)
        {

            Id = data.NID;
            Name = data.Name;
            Phone = data.Phone;
            OwnerName = data.OwnerName;
            Address = data.Address;
            DateOfBirth = data.DateOfBirth;
            Gender = (int)data.Gender;
            PictureUrl = data.PictureUrl;
        }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? PictureUrl { get; set; }
        public string? OwnerName { get; set; }
        public string? Address { get; set; }
    }
}
