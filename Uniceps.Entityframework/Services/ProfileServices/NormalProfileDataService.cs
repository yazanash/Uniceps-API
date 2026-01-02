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

namespace Uniceps.Entityframework.Services.ProfileServices
{
    public class NormalProfileDataService : IProfileDataService
    {
        private readonly AppDbContext _dbContext;

        public NormalProfileDataService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<NormalProfile> Create(NormalProfile entity)
        {
            //_contextFactory.Attach(entity.User!);
            EntityEntry<NormalProfile> CreatedResult = await _dbContext.Set<NormalProfile>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            NormalProfile? entity = await _dbContext.Set<NormalProfile>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<NormalProfile>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<NormalProfile> Update(NormalProfile entity)
        {
            _dbContext.Set<NormalProfile>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<NormalProfile> Get(Guid id)
        {
            NormalProfile? entity = await _dbContext.Set<NormalProfile>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<NormalProfile>> GetAll()
        {
            IEnumerable<NormalProfile>? entities = await _dbContext.Set<NormalProfile>().ToListAsync();
            return entities;
        }

        public async Task<NormalProfile> GetByUserId(string userid)
        {
            NormalProfile? entity = await _dbContext.Set<NormalProfile>().AsNoTracking().FirstOrDefaultAsync((e) => e.UserId == userid);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public Task<IEnumerable<NormalProfile>> GetByUserIdListAsync(string userid)
        {
            throw new NotImplementedException();
        }
    }
}
