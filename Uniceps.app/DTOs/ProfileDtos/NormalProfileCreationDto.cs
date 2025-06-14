using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.DTOs.ProfileDtos
{
    public class NormalProfileCreationDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Gender { get; set; }
        public string? PictureUrl { get; set; }
    }
}
