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
    public class SystemSubscriptionDataService : IMembershipDataService
    {
        private readonly AppDbContext _dbContext;

        public SystemSubscriptionDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<SystemSubscription> Create(SystemSubscription entity)
        {

            var pendingSubs = _dbContext.Set<SystemSubscription>()
        .Where(s => s.UserId == entity.UserId && !s.ISPaid && s.ProductId == entity.ProductId);

            _dbContext.Set<SystemSubscription>().RemoveRange(pendingSubs);

            var lastEndDate = await _dbContext.Set<SystemSubscription>()
        .Where(s => s.UserId == entity.UserId && s.ISPaid && s.ProductId == entity.ProductId)
        .MaxAsync(s => (DateTime?)s.EndDate);
            DateTime calculatedStart = (lastEndDate != null && lastEndDate > DateTime.UtcNow)
                                ? lastEndDate.Value
                                : DateTime.UtcNow;

            var duration = entity.EndDate.Subtract(entity.StartDate);

            entity.StartDate = calculatedStart;
            entity.EndDate = calculatedStart.Add(duration);

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
                throw new Exception($"subscription With Id {id} not exist");
            return entity!;
        }

        public async Task<SystemSubscription> GetActiveSubscriptionByAppId(string userid,int productId)
        {
            SystemSubscription? entity = await _dbContext.Set<SystemSubscription>().Where(x=>x.UserId == userid
            && x.ProductId == productId
            && x.StartDate<=DateTime.Now 
            && x.EndDate>=DateTime.Now&&x.ISPaid).AsNoTracking().FirstOrDefaultAsync();
            if (entity == null)
                throw new Exception("Subscription Not found");
            return entity!;
        }

        public async Task<IEnumerable<SystemSubscription>> GetByUserIdListAsync(string userid)
        {
            IEnumerable<SystemSubscription>? entity = await _dbContext.Set<SystemSubscription>().Where(x => x.UserId == userid
         && x.StartDate <= DateTime.Now
         && x.EndDate >= DateTime.Now && x.ISPaid == false).AsNoTracking().ToListAsync();
            
            return entity!;
        }

        public async Task<IEnumerable<SystemSubscription>> GetUnPaidSubscription()
        {
            IEnumerable<SystemSubscription>? entity = await _dbContext.Set<SystemSubscription>().Where(x => x.ISPaid == false).AsNoTracking().ToListAsync();
            return entity!;
        }

        public async Task<bool> HasUsedTrialForProduct(string userId, int productId)
        {
           return await _dbContext.Set<SystemSubscription>()
            .AnyAsync(s => s.UserId == userId && s.ProductId == productId);
        }

        public async Task<bool> SetSubscriptionAsPaid(Guid subId)
        {
            SystemSubscription? entity = await _dbContext.Set<SystemSubscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == subId);
            if (entity == null)
                return false;

            entity.ISPaid = true;
            entity.IsActive = true;
            _dbContext.Set<SystemSubscription>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<SystemSubscription> Update(SystemSubscription entity)
        {
            _dbContext.Set<SystemSubscription>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
