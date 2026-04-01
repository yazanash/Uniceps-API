using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class ExerciseUploadRequest
    {
        public string ExerciseId { get; set; } = string.Empty;
        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string MuscleGroupCode { get; set; } = string.Empty;
        public string MuscleHeadCode { get; set; } = string.Empty;
        public string MuscleAux1 { get; set; } = string.Empty;
        public string MuscleAux2 { get; set; } = string.Empty;
        public string MuscleAux3 { get; set; } = string.Empty;
        public string EquipmentCode { get; set; } = string.Empty;
        public string? Implementation { get; set; }
        public int Version { get; set; } = 1;
        public string Mechanism { get; set; } = string.Empty;
        public IFormFile? ImageFile { get; set; }
    }
}
