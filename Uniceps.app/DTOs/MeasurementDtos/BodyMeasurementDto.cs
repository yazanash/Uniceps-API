using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class BodyMeasurementDto
    {
        public int ApiId { get; set; }
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
        public BodyMeasurementDto(BodyMeasurement data)
        {
            ApiId = data.MId;
            HeightCm = data.HeightCm;
            WeightKg = data.WeightKg;
            // Core
            WaistCm = data.WaistCm;
            ChestCm = data.ChestCm;
            HipsCm = data.HipsCm;
            NeckCm = data.NeckCm;
            ShouldersCm = data.ShouldersCm;
            // Limbs
            LeftArmCm = data.LeftArmCm;
            RightArmCm = data.RightArmCm;
            LeftThighCm = data.LeftThighCm;
            RightThighCm = data.RightThighCm;
            LeftLegCm = data.LeftLegCm;
            RightLegCm = data.RightLegCm;
            MeasuredAt = data.MeasuredAt;
        }
    }
}
