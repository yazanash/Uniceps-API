using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.app.DTOs.ProfileDtos
{
    public class BusinessProfileDto
    {
        public int Id { get; set; }
        public string? BusinessName { get; set; }
        public string? OwnerName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? PictureUrl { get; set; }
        public int BusinessType { get; set; }
      
    }
}
