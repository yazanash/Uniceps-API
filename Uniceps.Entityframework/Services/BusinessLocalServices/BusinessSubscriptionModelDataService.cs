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
    public class BusinessSubscriptionModelDataService : IDataService<BusinessSubscriptionModel>, IUserQueryDataService<BusinessSubscriptionModel>
    {
        private readonly AppDbContext _contextFactory;


        public BusinessSubscriptionModelDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<BusinessSubscriptionModel> Create(BusinessSubscriptionModel entity)
        {
            EntityEntry<BusinessSubscriptionModel> CreatedResult = await _contextFactory.Set<BusinessSubscriptionModel>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            BusinessSubscriptionModel? entity = await _contextFactory.Set<BusinessSubscriptionModel>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<BusinessSubscriptionModel>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessSubscriptionModel> Get(int id)
        {
            BusinessSubscriptionModel? entity = await _contextFactory.Set<BusinessSubscriptionModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessSubscriptionModel>> GetAll()
        {
            IEnumerable<BusinessSubscriptionModel>? entities = await _contextFactory.Set<BusinessSubscriptionModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<BusinessSubscriptionModel>> GetAllByUser(string? userid)
        {
            IEnumerable<BusinessSubscriptionModel>? entities = await _contextFactory.Set<BusinessSubscriptionModel>()
                .Where(x=>x.BusinessId==userid).ToListAsync();
            return entities;
        }

        public async Task<BusinessSubscriptionModel> Update(BusinessSubscriptionModel entity)
        {
            _contextFactory.Set<BusinessSubscriptionModel>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
