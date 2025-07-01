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
    public class BusinessServiceModelDataService(AppDbContext dbContext) : IDataService<BusinessServiceModel>, IUserQueryDataService<BusinessServiceModel>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<BusinessServiceModel> Create(BusinessServiceModel entity)
        {
            EntityEntry<BusinessServiceModel> CreatedResult = await _dbContext.Set<BusinessServiceModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessServiceModel? entity = await _dbContext.Set<BusinessServiceModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BusinessServiceModel>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessServiceModel> Get(Guid id)
        {
            BusinessServiceModel? entity = await _dbContext.Set<BusinessServiceModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessServiceModel>> GetAll()
        {
            IEnumerable<BusinessServiceModel>? entities = await _dbContext.Set<BusinessServiceModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<BusinessServiceModel>> GetAllByUser(string? userid)
        {
            IEnumerable<BusinessServiceModel>? entities = await _dbContext.Set<BusinessServiceModel>()
                .Where(x=>x.BusinessId==userid||x.TrainerId==userid).ToListAsync();
            return entities;
        }

        public async Task<BusinessServiceModel> Update(BusinessServiceModel entity)
        {
            _dbContext.Set<BusinessServiceModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
