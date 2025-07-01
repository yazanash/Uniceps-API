using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Profile;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public class PlanDataService(AppDbContext dbContext) : IDataService<PlanModel>, IGetByTargetType<PlanModel>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<PlanModel> Create(PlanModel entity)
        {
            EntityEntry<PlanModel> CreatedResult = await _dbContext.Set<PlanModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            PlanModel? entity = await _dbContext.Set<PlanModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<PlanModel>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PlanModel> Get(Guid id)
        {
            PlanModel? entity = await _dbContext.Set<PlanModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<PlanModel>> GetAll()
        {
            IEnumerable<PlanModel>? entities = await _dbContext.Set<PlanModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlanModel>> GetAllByTarget(int target)
        {
            PlanTarget planTarget = (PlanTarget)target;
            IEnumerable<PlanModel>? entities = await _dbContext.Set<PlanModel>().Where(x=>x.TargetUserType == planTarget).ToListAsync();
            return entities;
        }

        public async Task<PlanModel> Update(PlanModel entity)
        {
            _dbContext.Set<PlanModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
