using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.NotificationSystemServices
{
    public class UserDeviceDataService(AppDbContext dbContext) : IDataService<UserDevice>, IUserQueryDataService<UserDevice>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<UserDevice> Create(UserDevice entity)
        {
            EntityEntry<UserDevice> CreatedResult = await _dbContext.Set<UserDevice>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            UserDevice? entity = await _dbContext.Set<UserDevice>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<UserDevice>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserDevice> Get(Guid id)
        {
            UserDevice? entity = await _dbContext.Set<UserDevice>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<UserDevice>> GetAll()
        {
            IEnumerable<UserDevice>? entities = await _dbContext.Set<UserDevice>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<UserDevice>> GetAllByUser(string? userid)
        {
            IEnumerable<UserDevice>? entities = await _dbContext.Set<UserDevice>().Where(x=>x.UserId==userid&&x.IsActive).ToListAsync();
            return entities;
        }

        public async Task<UserDevice> Update(UserDevice entity)
        {
            _dbContext.Set<UserDevice>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
