using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class MuscleGroupV2Response
    {

        private readonly MuscleGroupV2 MuscleGroup;
        private bool IsArabic;

        public MuscleGroupV2Response(MuscleGroupV2 muscleGroup, bool isArabic)
        {
            MuscleGroup = muscleGroup;
            IsArabic = isArabic;
        }
        public string Code => MuscleGroup.Code;
        public string Name => IsArabic ? MuscleGroup.NameAr : MuscleGroup.NameEn;
        public List<MuscleHeadResponse> MuscleHeads => MuscleGroup.Heads.Select(x=>new MuscleHeadResponse(x,IsArabic)).ToList();

    }
}
