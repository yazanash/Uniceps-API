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
    public class RoutineDayDataService(AppDbContext dbContext) : IDataService<Day>, IEntityQueryDataService<Day>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Day> Create(Day entity)
        {
            EntityEntry<Day> CreatedResult = await _dbContext.Set<Day>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            Day? entity = await _dbContext.Set<Day>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<Day>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Day> Update(Day entity)
        {
            _dbContext.Set<Day>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<Day> Get(Guid id)
        {
            Day? entity = await _dbContext.Set<Day>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Day>> GetAll()
        {
            IEnumerable<Day>? entities = await _dbContext.Set<Day>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Day>> GetAllById(Guid entityId)
        {
            IEnumerable<Day>? entities = await _dbContext.Set<Day>().Where(x => x.RoutineNID == entityId).ToListAsync();
            return entities;
        }
    }
}
