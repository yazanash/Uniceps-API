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
    public class ExerciseDataService(AppDbContext dbContext) : IIntDataService<Exercise>, IIntEntityQueryDataService<Exercise>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Exercise> Create(Exercise entity)
        {
            EntityEntry<Exercise> CreatedResult = await _dbContext.Set<Exercise>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
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
        public async Task<Exercise> Update(Exercise entity)
        {
            _dbContext.Set<Exercise>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
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
