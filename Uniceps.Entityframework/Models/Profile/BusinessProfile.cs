using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Models.Profile
{
    public class BusinessProfile: EntityBase
    {
        public string? BusinessName { get; set; }
        public string? OwnerName { get; set; }
        public string? Phone1 { get; set; }
        public string? Phone2 { get; set; }
        public string? PictureUrl { get; set; }
        public BusinessType BusinessType { get; set; }
        public string? UserId { get; set; }
        public AppUser? User{ get; set; }
    }
    public enum BusinessType
    {
        Coach = 1,
        Gym = 2,
        Organization = 3,
    }
}
