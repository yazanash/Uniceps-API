using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class IngredientCategoryResponse
    {
        private IngredientCategory IngredientCategory;

        public IngredientCategoryResponse(IngredientCategory ingredientCategory)
        {
            IngredientCategory = ingredientCategory;
        }
        public int ApiId => IngredientCategory.Id;
        public string EnglishName => IngredientCategory.EnglishName;
        public string ArabicName => IngredientCategory.ArabicName;
    }
}
