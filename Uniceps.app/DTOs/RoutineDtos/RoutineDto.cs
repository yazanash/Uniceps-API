using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class RoutineDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public List<DayDto>? RoutineDays { get; set; }
    }
}
