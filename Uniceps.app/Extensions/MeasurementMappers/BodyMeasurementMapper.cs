using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Extensions.MeasurementMappers
{
    public class BodyMeasurementMapper : IMapperExtension<BodyMeasurement, BodyMeasurementDto, BodyMeasurementCreationDto>
    {
        public BodyMeasurement FromCreationDto(BodyMeasurementCreationDto data)
        {
            BodyMeasurement bodyMeasurement = new BodyMeasurement();
            bodyMeasurement.HeightCm = data.HeightCm;
            bodyMeasurement.WeightKg = data.WeightKg;
            // Core
            bodyMeasurement.WaistCm = data.WaistCm;
            bodyMeasurement.ChestCm = data.ChestCm;
            bodyMeasurement.HipsCm = data.HipsCm;
            bodyMeasurement.NeckCm = data.NeckCm;
            bodyMeasurement.ShouldersCm = data.ShouldersCm;
            // Limbs
            bodyMeasurement.LeftArmCm = data.LeftArmCm;
            bodyMeasurement.RightArmCm = data.RightArmCm;
            bodyMeasurement.LeftThighCm = data.LeftThighCm;
            bodyMeasurement.RightThighCm = data.RightThighCm;
            bodyMeasurement.LeftLegCm = data.LeftLegCm;
            bodyMeasurement.RightLegCm = data.RightLegCm;
            return bodyMeasurement;
        }

        public BodyMeasurementDto ToDto(BodyMeasurement data)
        {
            BodyMeasurementDto bodyMeasurementDto = new BodyMeasurementDto();
            bodyMeasurementDto.Id = data.Id;
            bodyMeasurementDto.HeightCm = data.HeightCm;
            bodyMeasurementDto.WeightKg = data.WeightKg;
            // Core
            bodyMeasurementDto.WaistCm = data.WaistCm;
            bodyMeasurementDto.ChestCm = data.ChestCm;
            bodyMeasurementDto.HipsCm = data.HipsCm;
            bodyMeasurementDto.NeckCm = data.NeckCm;
            bodyMeasurementDto.ShouldersCm = data.ShouldersCm;
            // Limbs
            bodyMeasurementDto.LeftArmCm = data.LeftArmCm;
            bodyMeasurementDto.RightArmCm = data.RightArmCm;
            bodyMeasurementDto.LeftThighCm = data.LeftThighCm;
            bodyMeasurementDto.RightThighCm = data.RightThighCm;
            bodyMeasurementDto.LeftLegCm = data.LeftLegCm;
            bodyMeasurementDto.RightLegCm = data.RightLegCm;
            bodyMeasurementDto.MeasuredAt = data.MeasuredAt;
            return bodyMeasurementDto;  
        }
    }
}
