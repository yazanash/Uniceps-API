using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class ExerciseV2Response
    {
        private readonly ExerciseV2 Exercise;
        private readonly bool IsArabic;
        public ExerciseV2Response(ExerciseV2 exercise, bool isArabic)
        {
            Exercise = exercise;
            IsArabic = isArabic;
        }

        public string ExerciseId => Exercise.ExerciseId;
        public string Name => IsArabic ? Exercise.NameAr : Exercise.NameEn;
        public string MuscleGroupCode => Exercise.MuscleGroupCode;
        public string MuscleGroupName => IsArabic ? Exercise.MuscleGroupV2?.NameAr ?? "" : Exercise.MuscleGroupV2?.NameEn ?? "";
        public string MuscleHeadCode => Exercise.MuscleHeadCode;
        public string MuscleHeadName => IsArabic ? Exercise.MuscleHead?.NameAr ?? "" : Exercise.MuscleHead?.NameEn ?? "";
        public string EquipmentCode => Exercise.EquipmentCode;
        public string EquipmentName => IsArabic ? Exercise.Equipment?.NameAr ?? "" : Exercise.Equipment?.NameEn ?? "";
        public string? MuscleAux1 => Exercise.MuscleAux1;
        public string? MuscleAux2 => Exercise.MuscleAux2;
        public string? MuscleAux3 => Exercise.MuscleAux3;
        public string? Implementation => Exercise.Implementation;
        public int Version => Exercise.Version;
        public string? Mechanism => Exercise.Mechanism.ToString();
        public string ImageUrl => Exercise.ImageUrl;
        public int MediaVersion => Exercise.Version;
        public DateTime LastUpdated => Exercise.LastUpdated;
    }
}
