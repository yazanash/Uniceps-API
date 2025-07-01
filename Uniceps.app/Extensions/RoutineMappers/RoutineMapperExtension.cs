using Uniceps.app.DTOs.MuscleGroupDtos;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.RoutineMappers
{
    public class RoutineMapperExtension : IMapperExtension<Routine, RoutineDto, RoutineCreationDto>
    {
        IMapperExtension<Day, DayDto, DayCreationDto> _routinedaysMapper;

        public RoutineMapperExtension(IMapperExtension<Day, DayDto, DayCreationDto> routinedaysMapper)
        {
            _routinedaysMapper = routinedaysMapper;
        }

        public Routine FromCreationDto(RoutineCreationDto data)
        {
            Routine routine = new Routine();
            routine.Name = data.Name;
            routine.Description = data.Description;
            return routine;
        }

        public RoutineDto ToDto(Routine data)
        {
            RoutineDto routine = new RoutineDto();
            routine.Id = data.NID;
            routine.Name = data.Name;
            routine.Description = data.Description;
            if(data.Days.Count()>0)
                routine.RoutineDays!.AddRange(data.Days.Select(x => _routinedaysMapper.ToDto(x)));
            return routine;
        }
    }
}
