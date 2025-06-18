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
        private readonly AppDbContext _contextFactory;

        public SystemSubscriptionDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<SystemSubscription> Create(SystemSubscription entity)
        {
            EntityEntry<SystemSubscription> CreatedResult = await _contextFactory.Set<SystemSubscription>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            SystemSubscription? entity = await _contextFactory.Set<SystemSubscription>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<SystemSubscription>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<SystemSubscription> Get(int id)
        {
            SystemSubscription? entity = await _contextFactory.Set<SystemSubscription>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<SystemSubscription>> GetAll()
        {
            IEnumerable<SystemSubscription>? entities = await _contextFactory.Set<SystemSubscription>().ToListAsync();
            return entities;
        }

        public async Task<SystemSubscription> GetByUserId(string userid)
        {
            SystemSubscription? entity = await _contextFactory.Set<SystemSubscription>().Where(x=>x.UserId == userid
            && x.StartDate<=DateTime.Now 
            && x.EndDate>=DateTime.Now).AsNoTracking().FirstOrDefaultAsync();
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<SystemSubscription> Update(SystemSubscription entity)
        {
            _contextFactory.Set<SystemSubscription>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
