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
    public class PlayerModelDataService(AppDbContext dbContext) : IDataService<PlayerModel>,IUserQueryDataService<PlayerModel>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<PlayerModel> Create(PlayerModel entity)
        {
            EntityEntry<PlayerModel> CreatedResult = await _dbContext.Set<PlayerModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            PlayerModel? entity = await _dbContext.Set<PlayerModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<PlayerModel>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerModel> Get(Guid id)
        {
            PlayerModel? entity = await _dbContext.Set<PlayerModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
           
        }

        public async Task<IEnumerable<PlayerModel>> GetAll()
        {
            IEnumerable<PlayerModel>? entities = await _dbContext.Set<PlayerModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<PlayerModel>> GetAllByUser(string? userid)
        {
            IEnumerable<PlayerModel>? entities = await _dbContext.Set<PlayerModel>().Where(x=>x.BusinessId == userid).ToListAsync();
            return entities;
        }

        public async Task<PlayerModel> Update(PlayerModel entity)
        {
            _dbContext.Set<PlayerModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
