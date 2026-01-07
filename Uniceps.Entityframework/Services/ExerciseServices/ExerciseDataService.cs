using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.ExerciseServices
{
    public class ExerciseDataService(AppDbContext dbContext) : IExerciseDataService
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Exercise> UpsertAsync(Exercise entity)
        {
            var existing = await _dbContext.Exercises
        .FirstOrDefaultAsync(e => e.Name == entity.Name);

            if (existing != null)
            {
                existing.MuscleGroupId = entity.MuscleGroupId;
                existing.ImageUrl = entity.ImageUrl;

                _dbContext.Exercises.Update(existing);
                await _dbContext.SaveChangesAsync();
                return existing;
            }
            else
            {
                // إضافة تمرين جديد
                _dbContext.Exercises.Add(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
        }

        public async Task<bool> Delete(int id)
        {
            Exercise? entity = await _dbContext.Set<Exercise>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<Exercise>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<Exercise> Get(int id)
        {
            Exercise? entity = await _dbContext.Set<Exercise>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }
        public async Task<IEnumerable<Exercise>> GetAll()
        {
            IEnumerable<Exercise>? entities = await _dbContext.Set<Exercise>().ToListAsync();
            return entities;

        }
        public async Task<IEnumerable<Exercise>> GetAllById(int entityId)
        {
            IEnumerable<Exercise>? entities = await _dbContext.Set<Exercise>().Where(x => x.MuscleGroupId == entityId).ToListAsync();
            return entities;
        }
    }
}
