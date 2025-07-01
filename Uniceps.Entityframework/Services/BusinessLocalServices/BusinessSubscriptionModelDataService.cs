using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.Entityframework.Services.BusinessLocalServices
{
    public class BusinessSubscriptionModelDataService(AppDbContext dbContext) : IDataService<BusinessSubscriptionModel>, IUserQueryDataService<BusinessSubscriptionModel>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<BusinessSubscriptionModel> Create(BusinessSubscriptionModel entity)
        {
            EntityEntry<BusinessSubscriptionModel> CreatedResult = await _dbContext.Set<BusinessSubscriptionModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessSubscriptionModel? entity = await _dbContext.Set<BusinessSubscriptionModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BusinessSubscriptionModel>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessSubscriptionModel> Get(Guid id)
        {
            BusinessSubscriptionModel? entity = await _dbContext.Set<BusinessSubscriptionModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessSubscriptionModel>> GetAll()
        {
            IEnumerable<BusinessSubscriptionModel>? entities = await _dbContext.Set<BusinessSubscriptionModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<BusinessSubscriptionModel>> GetAllByUser(string? userid)
        {
            IEnumerable<BusinessSubscriptionModel>? entities = await _dbContext.Set<BusinessSubscriptionModel>()
                .Include(x=>x.BusinessService).AsNoTracking().Include(x=>x.PlayerModel).Where(x=>x.BusinessId==userid).ToListAsync();
            return entities;
        }

        public async Task<BusinessSubscriptionModel> Update(BusinessSubscriptionModel entity)
        {
            _dbContext.Set<BusinessSubscriptionModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
