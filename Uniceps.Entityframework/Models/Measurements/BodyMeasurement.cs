using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.AuthenticationModels;

namespace Uniceps.Entityframework.Models.Measurements
{
    public class BodyMeasurement
    {
        public Guid Id { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual AppUser? AppUser { get; set; }
        public string? BusinessId { get; set; }
        public double HeightCm { get; set; }
        public double WeightKg { get; set; }
        // Core
        public double WaistCm { get; set; }
        public double ChestCm { get; set; }
        public double HipsCm { get; set; }
        public double NeckCm { get; set; }
        public double ShouldersCm { get; set; }
        // Limbs
        public double LeftArmCm { get; set; }
        public double RightArmCm { get; set; }
        public double LeftThighCm { get; set; }
        public double RightThighCm { get; set; }
        public double LeftLegCm { get; set; }
        public double RightLegCm { get; set; }
        public DateTime MeasuredAt { get; set; } = DateTime.UtcNow;
    }
}
