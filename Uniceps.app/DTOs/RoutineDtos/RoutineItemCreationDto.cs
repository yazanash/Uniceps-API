using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class RoutineItemCreationDto
    {
        public int ExerciseId { get; set; }
        public int Order { get; set; }
        public Guid DayId {  get; set; }
        public virtual List<ItemSetCreationDto> Sets { get; set; } = new List<ItemSetCreationDto>();
    }
}
