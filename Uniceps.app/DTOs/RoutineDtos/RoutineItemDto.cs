using Uniceps.app.DTOs.ExerciseDtos;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class RoutineItemDto
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public ExerciseDto? Exercise { get; set; }
        public int DayId { get; set; }
        public int Order { get; set; }
        public virtual List<ItemSetDto> Sets { get; set; } = new List<ItemSetDto>();
    }
}
