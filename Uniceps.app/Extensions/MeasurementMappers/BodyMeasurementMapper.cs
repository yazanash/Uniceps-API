using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Extensions.MeasurementMappers
{
    public class BodyMeasurementMapper : IMapperExtension<BodyMeasurement, BodyMeasurementDto, BodyMeasurementCreationDto>
    {
        public BodyMeasurement FromCreationDto(BodyMeasurementCreationDto data)
        {
            throw new NotImplementedException();
        }

        public BodyMeasurementDto ToDto(BodyMeasurement data)
        {
            throw new NotImplementedException();
        }
    }
}
