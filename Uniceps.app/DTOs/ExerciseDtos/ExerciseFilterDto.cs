using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class ExerciseFilterDto
    {
        public string? MuscleGroupCode { get; set; }
        public string? MuscleHeadCode { get; set; }
        public string? EquipmentCode { get; set; }
        public ExerciseMechanism? Mechanism { get; set; }
        public string? SearchTerm { get; set; }
        public DateTime? LastSync { get; set; }
        public ExerciseFilter ToModel()
        {
            return new ExerciseFilter
            {
                EquipmentCode = !string.IsNullOrEmpty(EquipmentCode?.Trim()) ? EquipmentCode : null,
                MuscleGroupCode = !string.IsNullOrEmpty(MuscleGroupCode?.Trim()) ? MuscleGroupCode : null,
                MuscleHeadCode = !string.IsNullOrEmpty(MuscleHeadCode?.Trim()) ? MuscleHeadCode : null,
                SearchTerm = !string.IsNullOrEmpty(SearchTerm?.Trim()) ? SearchTerm : null,
                Mechanism = Mechanism,
                 LastSync= LastSync
            };
        }
    }
}
