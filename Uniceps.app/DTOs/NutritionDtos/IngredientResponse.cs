using System.ComponentModel.DataAnnotations.Schema;
using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.app.DTOs.NutritionDtos
{
    public class IngredientResponse
    {
        public Ingredient Ingredient;
        public bool IsArabic;
        public IngredientResponse(Ingredient ingredient, bool isArabic)
        {
            Ingredient = ingredient;
            IsArabic = isArabic;
        }

        public string ApiId =>Ingredient.Id.ToString();
        public string Name => IsArabic? Ingredient.ArabicName : Ingredient.EnglishName;
        public bool IsUserGenerated => Ingredient.IsUserGenerated;
        public int CategoryId => Ingredient.CategoryId;
        public string? CategoryName => IsArabic? Ingredient.Category?.ArabicName: Ingredient.Category?.EnglishName;
        public float DefaultServingInGrams => Ingredient.DefaultServingInGrams;
        public float Calories => Ingredient.Calories;
        public float Protein => Ingredient.Protein;
        public float Carbs => Ingredient.Carbs;
        public float Fats=> Ingredient.Fats;
        public DateTime CreatedAt  => Ingredient.CreatedAt;
        public DateTime UpdatedAt => Ingredient.UpdatedAt;
    }
}
