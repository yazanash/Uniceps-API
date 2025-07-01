using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class RoutineItemDto
    {
        public Guid Id { get; set; }
        public int ExerciseId { get; set; }
        public ExerciseDto? Exercise { get; set; }
        public Guid DayId { get; set; }
        public int Order { get; set; }
        public virtual List<ItemSetDto> Sets { get; set; } = new List<ItemSetDto>();
    }
}
