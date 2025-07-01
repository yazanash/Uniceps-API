using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.MuscleGroupServices
{
    public class MuscleGroupDataService(AppDbContext dbContext) : IIntDataService<MuscleGroup>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<MuscleGroup> Create(MuscleGroup entity)
        {
            EntityEntry<MuscleGroup> CreatedResult = await _dbContext.Set<MuscleGroup>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            MuscleGroup? entity = await _dbContext.Set<MuscleGroup>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<MuscleGroup>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<MuscleGroup> Update(MuscleGroup entity)
        {
            _dbContext.Set<MuscleGroup>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<MuscleGroup> Get(int id)
        {
            MuscleGroup? entity = await _dbContext.Set<MuscleGroup>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<MuscleGroup>> GetAll()
        {
            IEnumerable<MuscleGroup>? entities = await _dbContext.Set<MuscleGroup>().ToListAsync();
            return entities;

        }
    }
}
