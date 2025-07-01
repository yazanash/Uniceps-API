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
    public class RoutineDataService(AppDbContext dbContext) : IDataService<Routine>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<Routine> Create(Routine entity)
        {
            EntityEntry<Routine> CreatedResult = await _dbContext.Set<Routine>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            Routine? entity = await _dbContext.Set<Routine>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<Routine>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<Routine> Update(Routine entity)
        {
            _dbContext.Set<Routine>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<Routine> Get(Guid id)
        {
            Routine? entity = await _dbContext.Set<Routine>().AsNoTracking()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Routine>> GetAll()
        {
            IEnumerable<Routine>? entities = await _dbContext.Set<Routine>()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).ToListAsync();
            return entities;

        }
    }
}
