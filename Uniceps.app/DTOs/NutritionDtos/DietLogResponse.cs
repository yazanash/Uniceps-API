using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class DietLogResponse
    {
        private DietLog DietLog;

        public DietLogResponse(DietLog dietLog)
        {
            DietLog = dietLog;
        }
        public int ApiId => DietLog.Id;
        public string IngredientName => DietLog.IngredientName;
        public float TotalGrams => DietLog.TotalGrams;
        public float Calories => DietLog.Calories;
        public float Protein => DietLog.Protein;
        public float Carbs => DietLog.Carbs;
        public float Fats => DietLog.Fats;
        public DateTime Timestamp => DietLog.Timestamp;
    }
}
