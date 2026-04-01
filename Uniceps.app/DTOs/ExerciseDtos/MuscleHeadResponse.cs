using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class MuscleHeadResponse
    {
        private readonly MuscleHead MuscleHead;
        private bool IsArabic;

        public MuscleHeadResponse(MuscleHead muscleHead, bool isArabic)
        {
            MuscleHead = muscleHead;
            IsArabic = isArabic;
        }

        public string Code => MuscleHead.Code;
        public string MuscleGroupCode => MuscleHead.MuscleGroupCode;
        public string Name => IsArabic? MuscleHead.NameAr:MuscleHead.NameEn;
    }
}
