using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class DayDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Guid RoutineId { get; set; }
        public List<RoutineItemDto>? RoutineItems {get;set;}
    }
}
