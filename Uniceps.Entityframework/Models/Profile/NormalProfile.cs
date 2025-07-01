using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.Entityframework.Models.Profile
{
    public class NormalProfile:EntityBase
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public GenderType Gender { get; set; }
        public string? PictureUrl { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public AppUser? User { get; set; }
    }
    public enum GenderType
    {
        Male = 0,
        Female = 1
    }
}
