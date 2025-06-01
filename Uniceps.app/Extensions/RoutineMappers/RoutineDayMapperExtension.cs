using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.RoutineMappers
{
    public class RoutineDayMapperExtension : IMapperExtension<Day, DayDto, DayCreationDto>
    {

        IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto> _routineItemMapper;

        public RoutineDayMapperExtension(IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto> routineItemMapper)
        {
            _routineItemMapper = routineItemMapper;
        }

        public Day FromCreationDto(DayCreationDto data)
        {
            Day day = new();
            day.Name = data.Name;
            return day;
        }

        public DayDto ToDto(Day data)
        {
            DayDto day = new();
            day.Id = data.Id;
            day.Name = data.Name;
            day.Id = data.Id;
            if (data.RoutineItems.Count() > 0)
                day.RoutineItems.AddRange(data.RoutineItems.Select(x=>_routineItemMapper.ToDto(x)));
            return day;
        }
    }
}
