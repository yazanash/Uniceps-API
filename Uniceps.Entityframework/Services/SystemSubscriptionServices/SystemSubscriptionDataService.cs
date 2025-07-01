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
    public class SystemSubscriptionDataService : IDataService<SystemSubscription>,IGetByUserId<SystemSubscription>
    {
        private readonly AppDbContext _dbContext;

        public SystemSubscriptionDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SystemSubscription> Create(SystemSubscription entity)
        {
            EntityEntry<SystemSubscription> CreatedResult = await _dbContext.Set<SystemSubscription>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            SystemSubscription? entity = await _dbContext.Set<SystemSubscription>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<SystemSubscription>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<SystemSubscription> Get(Guid id)
        {
            SystemSubscription? entity = await _dbContext.Set<SystemSubscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<SystemSubscription>> GetAll()
        {
            IEnumerable<SystemSubscription>? entities = await _dbContext.Set<SystemSubscription>().ToListAsync();
            return entities;
        }

        public async Task<SystemSubscription> GetByUserId(string userid)
        {
            SystemSubscription? entity = await _dbContext.Set<SystemSubscription>().Where(x=>x.UserId == userid
            && x.StartDate<=DateTime.Now 
            && x.EndDate>=DateTime.Now).AsNoTracking().FirstOrDefaultAsync();
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<SystemSubscription> Update(SystemSubscription entity)
        {
            _dbContext.Set<SystemSubscription>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
