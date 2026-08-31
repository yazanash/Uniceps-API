using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.Entityframework.Services.NutritionServices
{
    public interface IIngredientDataService
    {
         public Task<Ingredient> Create(Ingredient entity);
        public Task<Ingredient> Update(Ingredient entity);
        public Task<Ingredient> UpsertAsync(Ingredient entity);
        public Task<bool> Delete(Guid id);
        public Task<IEnumerable<Ingredient>> GetAll(string? userId=null, DateTime? lastSync = null);
        public Task<Ingredient> Get(Guid id);
        public Task<IngredientCategory> CreateCategory(IngredientCategory entity);
        public Task<IngredientCategory> UpdateCategory(IngredientCategory entity);
        public Task<bool> DeleteCategoryAsync(int id);
        public Task<IEnumerable<Ingredient>> GetAllByCategory(int categoryId);

        public Task<IEnumerable<IngredientCategory>> GetAllCategories();
    }
}
