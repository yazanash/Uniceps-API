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
    public class RoutineItemDataService : IDataService<RoutineItem>, IEntityQueryDataService<RoutineItem>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineItemDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<RoutineItem> Create(RoutineItem entity)
        {
            EntityEntry<RoutineItem> CreatedResult = await _contextFactory.Set<RoutineItem>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            RoutineItem? entity = await _contextFactory.Set<RoutineItem>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<RoutineItem>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<RoutineItem> Update(RoutineItem entity)
        {
            _contextFactory.Set<RoutineItem>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
        public async Task<RoutineItem> Get(int id)
        {
            RoutineItem? entity = await _contextFactory.Set<RoutineItem>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<RoutineItem>> GetAll()
        {
            IEnumerable<RoutineItem>? entities = await _contextFactory.Set<RoutineItem>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<RoutineItem>> GetAllById(int entityId)
        {
            IEnumerable<RoutineItem>? entities = await _contextFactory.Set<RoutineItem>().Where(x => x.DayId == entityId).ToListAsync();
            return entities;
        }
    }
}
