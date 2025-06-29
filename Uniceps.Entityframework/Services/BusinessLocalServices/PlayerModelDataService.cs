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
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.BusinessLocalServices
{
    public class PlayerModelDataService : IDataService<PlayerModel>,IUserQueryDataService<PlayerModel>
    {
        private readonly AppDbContext _contextFactory;


        public PlayerModelDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<PlayerModel> Create(PlayerModel entity)
        {
            EntityEntry<PlayerModel> CreatedResult = await _contextFactory.Set<PlayerModel>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            PlayerModel? entity = await _contextFactory.Set<PlayerModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<PlayerModel>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerModel> Get(Guid id)
        {
            PlayerModel? entity = await _contextFactory.Set<PlayerModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
           
        }

        public async Task<IEnumerable<PlayerModel>> GetAll()
        {
            IEnumerable<PlayerModel>? entities = await _contextFactory.Set<PlayerModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlayerModel>> GetAllByUser(string? userid)
        {
            IEnumerable<PlayerModel>? entities = await _contextFactory.Set<PlayerModel>().Where(x=>x.BusinessId == userid).ToListAsync();
            return entities;
        }

        public async Task<PlayerModel> Update(PlayerModel entity)
        {
            _contextFactory.Set<PlayerModel>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
