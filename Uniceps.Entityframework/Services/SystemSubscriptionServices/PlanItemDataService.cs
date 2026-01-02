using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public class PlanItemDataService(AppDbContext dbContext) : IIntDataService<PlanItem>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<PlanItem> Create(PlanItem entity)
        {
            EntityEntry<PlanItem> CreatedResult = await _dbContext.Set<PlanItem>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            PlanItem? entity = await _dbContext.Set<PlanItem>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<PlanItem>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PlanItem> Get(int id)
        {
            PlanItem? entity = await _dbContext.Set<PlanItem>().AsNoTracking().Include(x=>x.PlanModel).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<PlanItem>> GetAll()
        {
            IEnumerable<PlanItem>? entities = await _dbContext.Set<PlanItem>().ToListAsync();
            return entities;
        }

        public async Task<PlanItem> Update(PlanItem entity)
        {
            _dbContext.Set<PlanItem>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
