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
    public class MuscleGroupDataService : IIntDataService<MuscleGroup>
    {
        private readonly AppDbContext _contextFactory;

        public MuscleGroupDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<MuscleGroup> Create(MuscleGroup entity)
        {
            EntityEntry<MuscleGroup> CreatedResult = await _contextFactory.Set<MuscleGroup>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            MuscleGroup? entity = await _contextFactory.Set<MuscleGroup>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<MuscleGroup>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<MuscleGroup> Update(MuscleGroup entity)
        {
            _contextFactory.Set<MuscleGroup>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
        public async Task<MuscleGroup> Get(int id)
        {
            MuscleGroup? entity = await _contextFactory.Set<MuscleGroup>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<MuscleGroup>> GetAll()
        {
            IEnumerable<MuscleGroup>? entities = await _contextFactory.Set<MuscleGroup>().ToListAsync();
            return entities;

        }
    }
}
