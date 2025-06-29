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
    public class BusinessServiceModelDataService : IDataService<BusinessServiceModel>, IUserQueryDataService<BusinessServiceModel>
    {
        private readonly AppDbContext _contextFactory;


        public BusinessServiceModelDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<BusinessServiceModel> Create(BusinessServiceModel entity)
        {
            EntityEntry<BusinessServiceModel> CreatedResult = await _contextFactory.Set<BusinessServiceModel>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessServiceModel? entity = await _contextFactory.Set<BusinessServiceModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<BusinessServiceModel>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessServiceModel> Get(Guid id)
        {
            BusinessServiceModel? entity = await _contextFactory.Set<BusinessServiceModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessServiceModel>> GetAll()
        {
            IEnumerable<BusinessServiceModel>? entities = await _contextFactory.Set<BusinessServiceModel>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<BusinessServiceModel>> GetAllByUser(string? userid)
        {
            IEnumerable<BusinessServiceModel>? entities = await _contextFactory.Set<BusinessServiceModel>()
                .Where(x=>x.BusinessId==userid||x.TrainerId==userid).ToListAsync();
            return entities;
        }

        public async Task<BusinessServiceModel> Update(BusinessServiceModel entity)
        {
            _contextFactory.Set<BusinessServiceModel>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
