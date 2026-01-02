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
    public class CashPaymentRequestDataService(AppDbContext dbContext) : IIntDataService<CashPaymentRequest>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<CashPaymentRequest> Create(CashPaymentRequest entity)
        {
            EntityEntry<CashPaymentRequest> CreatedResult = await _dbContext.Set<CashPaymentRequest>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            CashPaymentRequest? entity = await _dbContext.Set<CashPaymentRequest>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<CashPaymentRequest>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<CashPaymentRequest> Get(int id)
        {
            CashPaymentRequest? entity = await _dbContext.Set<CashPaymentRequest>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<CashPaymentRequest>> GetAll()
        {
            IEnumerable<CashPaymentRequest>? entities = await _dbContext.Set<CashPaymentRequest>().ToListAsync();
            return entities;
        }

        public async Task<CashPaymentRequest> Update(CashPaymentRequest entity)
        {
            _dbContext.Set<CashPaymentRequest>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
