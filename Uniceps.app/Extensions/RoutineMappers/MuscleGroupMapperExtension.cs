using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.RoutineMappers
{
    public class MuscleGroupMapperExtension : IMapperExtension<MuscleGroup, MuscleGroupDto, MuscleGroupCreateDto>
    {
        public MuscleGroup FromCreationDto(MuscleGroupCreateDto data)
        {
            return new MuscleGroup()
            {
                EngName = data.EngName,
                Name = data.Name,
            };
        }

        public MuscleGroupDto ToDto(MuscleGroup data)
        {
            return new MuscleGroupDto()
            {
                Id = data.Id,
                Name = data.Name,
                EngName = data.EngName,
            };
        }
    }
}
