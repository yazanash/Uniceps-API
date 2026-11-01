using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class BodyMeasurementCreationDto
    {
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
        public BodyMeasurement ToModel()
        {
            BodyMeasurement bodyMeasurement = new BodyMeasurement();
            bodyMeasurement.HeightCm = HeightCm;
            bodyMeasurement.WeightKg = WeightKg;
            bodyMeasurement.WaistCm = WaistCm;
            bodyMeasurement.ChestCm = ChestCm;
            bodyMeasurement.HipsCm = HipsCm;
            bodyMeasurement.NeckCm = NeckCm;
            bodyMeasurement.ShouldersCm = ShouldersCm;
            bodyMeasurement.LeftArmCm = LeftArmCm;
            bodyMeasurement.RightArmCm = RightArmCm;
            bodyMeasurement.LeftThighCm = LeftThighCm;
            bodyMeasurement.RightThighCm = RightThighCm;
            bodyMeasurement.LeftLegCm = LeftLegCm;
            bodyMeasurement.RightLegCm = RightLegCm;
            bodyMeasurement.MeasuredAt = MeasuredAt;
            return bodyMeasurement;
        }
    }
}
