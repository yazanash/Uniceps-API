using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class EquipmentsResponse
    {
        private readonly Equipment Equipment;
        private bool IsArabic;
        public EquipmentsResponse(Equipment equipment, bool isArabic)
        {
            Equipment = equipment;
            IsArabic = isArabic;
        }

        public string Code => Equipment.Code;
        public string Name => IsArabic ? Equipment.NameAr : Equipment.NameEn;
    }
}
