using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class DayDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RoutineId { get; set; }
        public List<RoutineItemDto>? RoutineItems {get;set;}
    }
}
