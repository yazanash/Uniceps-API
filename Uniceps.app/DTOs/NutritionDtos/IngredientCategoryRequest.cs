
using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class IngredientCategoryRequest
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        internal IngredientCategory ToModel()
        {
            return new IngredientCategory
            {
                ArabicName = ArabicName,
                EnglishName = EnglishName
            };
        }
    }
}
