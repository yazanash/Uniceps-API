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
    public class PlanDataService : IDataService<PlanModel>, IGetByTargetType<PlanModel>
    {
        private readonly AppDbContext _contextFactory;

        public PlanDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<PlanModel> Create(PlanModel entity)
        {
            EntityEntry<PlanModel> CreatedResult = await _contextFactory.Set<PlanModel>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            PlanModel? entity = await _contextFactory.Set<PlanModel>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<PlanModel>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<PlanModel> Get(int id)
        {
            PlanModel? entity = await _contextFactory.Set<PlanModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<PlanModel>> GetAll()
        {
            IEnumerable<PlanModel>? entities = await _contextFactory.Set<PlanModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlanModel>> GetAllByTarget(int target)
        {
            PlanTarget planTarget = (PlanTarget)target;
            IEnumerable<PlanModel>? entities = await _contextFactory.Set<PlanModel>().Where(x=>x.TargetUserType == planTarget).ToListAsync();
            return entities;
        }

        public async Task<PlanModel> Update(PlanModel entity)
        {
            _contextFactory.Set<PlanModel>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
