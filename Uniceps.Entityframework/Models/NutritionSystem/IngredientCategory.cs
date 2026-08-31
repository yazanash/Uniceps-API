using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.NutritionSystem
{
    public class IngredientCategory
    {
        public int Id { get; set; } 
        public string EnglishName { get; set; }=string.Empty;
        public string ArabicName { get; set; } = string.Empty;

    }
}
