using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.app.DTOs.RoutineDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.Extensions.RoutineMappers
{
    public class RoutineItemMapperExtension : IMapperExtension<RoutineItem, RoutineItemDto, RoutineItemCreationDto>
    {
        IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> _exerciseMapper;

        public RoutineItemMapperExtension(IMapperExtension<Exercise, ExerciseDto, ExerciseCreateDto> exerciseMapper)
        {
            _exerciseMapper = exerciseMapper;
        }

        public RoutineItem FromCreationDto(RoutineItemCreationDto data)
        {
            RoutineItem routineItem = new();
            routineItem.Order = data.Order;
            routineItem.ExerciseId = data.ExerciseId;
            foreach (ItemSetCreationDto itemSetCreationDto in data.Sets)
            {
                routineItem.Sets.Add(new ItemSet
                {
                    RoundIndex = itemSetCreationDto.RoundIndex,
                    Repetition = itemSetCreationDto.Repetition
                });
            }
            return routineItem;
        }

        public RoutineItemDto ToDto(RoutineItem data)
        {
            RoutineItemDto routineItem = new();
            routineItem.Id = data.Id;
            routineItem.Order = data.Order;
            routineItem.ExerciseId = data.ExerciseId;
            routineItem.Exercise = _exerciseMapper.ToDto(data.Exercise);

            foreach (ItemSet itemSet in data.Sets)
            {
                routineItem.Sets.Add(new ItemSetDto
                {
                    RoundIndex = itemSet.RoundIndex,
                    Repetition = itemSet.Repetition,
                    RoutineItemId = itemSet.RoutineItemId,
                    Id = itemSet.Id
                });
            }
            return routineItem;
        }
    }
}
