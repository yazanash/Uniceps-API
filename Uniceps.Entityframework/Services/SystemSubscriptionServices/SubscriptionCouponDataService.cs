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
    public class SubscriptionCouponDataService(AppDbContext dbContext) : ICouponDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<SubscriptionCoupon> Create(SubscriptionCoupon entity)
        {
            EntityEntry<SubscriptionCoupon> CreatedResult = await _dbContext.Set<SubscriptionCoupon>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id, string? userId = null)
        {
            SubscriptionCoupon? entity = await _dbContext.Set<SubscriptionCoupon>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<SubscriptionCoupon>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<SubscriptionCoupon> Get(int id)
        {
            SubscriptionCoupon? entity = await _dbContext.Set<SubscriptionCoupon>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<SubscriptionCoupon>> GetAll()
        {
            IEnumerable<SubscriptionCoupon>? entities = await _dbContext.Set<SubscriptionCoupon>().ToListAsync();
            return entities;
        }

        public  async Task<SubscriptionCoupon> GetByCodeAsync(string code)
        {
            SubscriptionCoupon? entity = await _dbContext.Set<SubscriptionCoupon>().AsNoTracking().FirstOrDefaultAsync((e) => e.Code == code&&e.ExpirationAt >= DateTime.UtcNow);
            if (entity == null)
                throw new Exception();
            if (entity.ExpirationAt < DateTime.UtcNow)
                throw new Exception("Coupon Expired");

            return entity;
        }

        public async Task<SubscriptionCoupon> Update(SubscriptionCoupon entity)
        {
            _dbContext.Set<SubscriptionCoupon>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
