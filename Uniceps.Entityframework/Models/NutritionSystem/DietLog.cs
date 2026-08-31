using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.NutritionSystem
{
    public class DietLog
    {
        public int Id { get; set; }
        public string? UserId {  get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public float TotalGrams { get; set; } = 100;
        public float Calories { get; set; }
        public float Protein { get; set; }
        public float Carbs { get; set; }
        public float Fats { get; set; }
        public DateTime Timestamp { get; set; }

    }
}
