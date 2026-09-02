using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.NutritionSystem
{
    public class Ingredient
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EnglishName { get; set; } = string.Empty;
        public string ArabicName { get; set; } = string.Empty;
        [ForeignKey("CategoryId")]
        public IngredientCategory? Category { get; set; }
        public int CategoryId  { get; set; }
        public float DefaultServingInGrams { get; set; } = 100f;
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }
        public string? UserId{ get; set; }
        public bool IsUserGenerated { get; set; } = false;
        public bool IsVerified { get; set; } = false;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
