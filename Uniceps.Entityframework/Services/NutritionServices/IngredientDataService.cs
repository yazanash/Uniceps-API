using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.NutritionServices
{
    public class IngredientDataService(AppDbContext dbContext) : IIngredientDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<Ingredient> Create(Ingredient entity)
        {
            EntityEntry<Ingredient> CreatedResult = await _dbContext.Set<Ingredient>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }
        public async Task<Ingredient> UpsertAsync(Ingredient entity)
        {
            var existingIngredient = await _dbContext.Set<Ingredient>()
        .FirstOrDefaultAsync(s => s.Id == entity.Id);
            if (existingIngredient != null)
            {
                EntityEntry<Ingredient> CreatedResult = await _dbContext.Set<Ingredient>().AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return CreatedResult.Entity;
            }
            entity.UpdatedAt = DateTime.UtcNow;
            _dbContext.Set<Ingredient>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;

        }
        public async Task<IngredientCategory> CreateCategory(IngredientCategory entity)
        {
            EntityEntry<IngredientCategory> CreatedResult = await _dbContext.Set<IngredientCategory>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public Task<bool> Delete(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteCategoryAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Ingredient> Get(Guid id)
        {
            Ingredient? entity = await _dbContext.Set<Ingredient>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Ingredient>> GetAll(string? userId = null, DateTime? lastSync = null)
        {
            IQueryable<Ingredient> query = _dbContext.Set<Ingredient>();

            if (lastSync.HasValue)
            {
                DateTime cleanLastSync = lastSync.Value.AddSeconds(1);
                query = query.Where(x => x.IsVerified && x.UpdatedAt > cleanLastSync);
            }
            else
            {
                query = query.Where(x => x.IsVerified || (!string.IsNullOrEmpty(userId) && x.UserId == userId));
            }

            return await query.AsNoTracking()
                        .Include(x => x.Category)
                        .ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetAllByCategory(int categoryId)
        {
            IEnumerable<Ingredient>? entities = await _dbContext.Set<Ingredient>().Where(x => x.CategoryId == categoryId).ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<IngredientCategory>> GetAllCategories()
        {
            IEnumerable<IngredientCategory>? entities = await _dbContext.Set<IngredientCategory>().ToListAsync();
            return entities;
        }

        public async Task<Ingredient> Update(Ingredient entity)
        {
            _dbContext.Set<Ingredient>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<IngredientCategory> UpdateCategory(IngredientCategory entity)
        {
            _dbContext.Set<IngredientCategory>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }


    }
}
