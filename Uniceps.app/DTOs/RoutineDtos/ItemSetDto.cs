using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class ItemSetDto
    {
        public Guid Id { get; set; }
        public int RoundIndex { get; set; }
        public int Repetition { get; set; }
        public Guid RoutineItemId { get; set; }
    }
}
