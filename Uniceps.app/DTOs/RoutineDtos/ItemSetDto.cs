using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class ItemSetDto
    {
        public int Id { get; set; }
        public int RoundIndex { get; set; }
        public int Repetition { get; set; }
        public int RoutineItemId { get; set; }
    }
}
