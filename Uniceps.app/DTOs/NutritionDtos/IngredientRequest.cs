using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class IngredientRequest
    {
        public Guid? ApiId { get; set; }
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public float DefaultServingInGrams { get; set; }
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }

        internal Ingredient ToModel()
        {
            return new Ingredient
            {
                ArabicName = !string.IsNullOrEmpty(ArabicName)?ArabicName:Name,
                Calories = Calories,
                Carbs = Carbs,
                CategoryId = CategoryId,
                CreatedAt = DateTime.UtcNow,
                EnglishName = !string.IsNullOrEmpty(EnglishName) ? EnglishName : Name,
                Fats = Fats,
                DefaultServingInGrams = DefaultServingInGrams,
                Protein = Protein,
            };
        }
    }
}
