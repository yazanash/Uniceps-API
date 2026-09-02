using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class DietLogRequest
    {
        public int? ApiId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public float TotalGrams { get; set; } = 100;
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }
        public DateTime Timestamp { get; set; }
        public DietLog ToModel()
        {
            return new DietLog
            {
                Calories = Calories,
                Carbs = Carbs,
                Timestamp = Timestamp,
                Fats = Fats,
                IngredientName = IngredientName,
                Protein = Protein,
                TotalGrams = TotalGrams,
                Id = ApiId??0
            };
        }
    }
}
