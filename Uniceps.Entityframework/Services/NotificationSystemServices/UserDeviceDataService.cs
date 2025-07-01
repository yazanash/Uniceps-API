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
    public class UserDeviceDataService : IDataService<UserDevice>, IUserQueryDataService<UserDevice>
    {
        private readonly AppDbContext _contextFactory;

        public UserDeviceDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<UserDevice> Create(UserDevice entity)
        {
            EntityEntry<UserDevice> CreatedResult = await _contextFactory.Set<UserDevice>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            UserDevice? entity = await _contextFactory.Set<UserDevice>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<UserDevice>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<UserDevice> Get(Guid id)
        {
            UserDevice? entity = await _contextFactory.Set<UserDevice>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<UserDevice>> GetAll()
        {
            IEnumerable<UserDevice>? entities = await _contextFactory.Set<UserDevice>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<UserDevice>> GetAllByUser(string? userid)
        {
            IEnumerable<UserDevice>? entities = await _contextFactory.Set<UserDevice>().Where(x=>x.UserId==userid&&x.IsActive).ToListAsync();
            return entities;
        }

        public async Task<UserDevice> Update(UserDevice entity)
        {
            _contextFactory.Set<UserDevice>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
