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
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.ProfileServices
{
    public class BusinessProfileDataService : IDataService<BusinessProfile>, IGetByUserId<BusinessProfile>
    {
        private readonly AppDbContext _contextFactory;

        public BusinessProfileDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<BusinessProfile> Create(BusinessProfile entity)
        {
            EntityEntry<BusinessProfile> CreatedResult = await _contextFactory.Set<BusinessProfile>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessProfile? entity = await _contextFactory.Set<BusinessProfile>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<BusinessProfile>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessProfile> Update(BusinessProfile entity)
        {
            _contextFactory.Set<BusinessProfile>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    
        public async Task<BusinessProfile> Get(Guid id)
        {
            BusinessProfile? entity = await _contextFactory.Set<BusinessProfile>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessProfile>> GetAll()
        {
            IEnumerable<BusinessProfile>? entities = await _contextFactory.Set<BusinessProfile>().ToListAsync();
            return entities;
        }

        public async Task<BusinessProfile> GetByUserId(string userid)
        {
            BusinessProfile? entity = await _contextFactory.Set<BusinessProfile>().AsNoTracking().FirstOrDefaultAsync((e) => e.UserId == userid);
            if (entity == null)
                throw new Exception();
            return entity!;
        }
    }
}
