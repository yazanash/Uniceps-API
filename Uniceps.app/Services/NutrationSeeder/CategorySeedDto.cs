namespace Uniceps.app.Services.NutrationSeeder
{
    public class CategorySeedDto
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public List<IngredientSeedDto> Ingredients { get; set; } = new();
    }
    public class IngredientSeedDto
    {
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        public float DefaultServingInGrams { get; set; }
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }
    }
}
