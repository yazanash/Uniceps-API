using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.RoutineServices
{
    public class RoutineItemDataService(AppDbContext dbContext) : IDataService<RoutineItem>, IEntityQueryDataService<RoutineItem>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<RoutineItem> Create(RoutineItem entity)
        {
            EntityEntry<RoutineItem> CreatedResult = await _dbContext.Set<RoutineItem>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            RoutineItem? entity = await _dbContext.Set<RoutineItem>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<RoutineItem>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RoutineItem> Update(RoutineItem entity)
        {
            _dbContext.Set<RoutineItem>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<RoutineItem> Get(Guid id)
        {
            RoutineItem? entity = await _dbContext.Set<RoutineItem>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<RoutineItem>> GetAll()
        {
            IEnumerable<RoutineItem>? entities = await _dbContext.Set<RoutineItem>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<RoutineItem>> GetAllById(Guid entityId)
        {
            IEnumerable<RoutineItem>? entities = await _dbContext.Set<RoutineItem>().Where(x => x.DayNID == entityId).ToListAsync();
            return entities;
        }
    }
}
