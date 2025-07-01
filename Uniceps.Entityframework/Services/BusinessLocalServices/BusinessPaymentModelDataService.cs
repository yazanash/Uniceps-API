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
    public class BusinessPaymentModelDataService(AppDbContext dbContext) : IDataService<BusinessPaymentModel>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<BusinessPaymentModel> Create(BusinessPaymentModel entity)
        {
            EntityEntry<BusinessPaymentModel> CreatedResult = await _dbContext.Set<BusinessPaymentModel>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessPaymentModel? entity = await _dbContext.Set<BusinessPaymentModel>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BusinessPaymentModel>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessPaymentModel> Get(Guid id)
        {
            BusinessPaymentModel? entity = await _dbContext.Set<BusinessPaymentModel>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessPaymentModel>> GetAll()
        {
            IEnumerable<BusinessPaymentModel>? entities = await _dbContext.Set<BusinessPaymentModel>().ToListAsync();
            return entities;
        }

        public async Task<BusinessPaymentModel> Update(BusinessPaymentModel entity)
        {
            _dbContext.Set<BusinessPaymentModel>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
