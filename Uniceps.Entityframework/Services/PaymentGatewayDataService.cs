using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services
{
    public class PaymentGatewayDataService(AppDbContext dbContext) : IIntDataService<PaymentGateway>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<PaymentGateway> Create(PaymentGateway entity)
        {
            EntityEntry<PaymentGateway> CreatedResult = await _dbContext.Set<PaymentGateway>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            PaymentGateway? entity = await _dbContext.Set<PaymentGateway>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<PaymentGateway>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<PaymentGateway> Get(int id)
        {
            PaymentGateway? entity = await _dbContext.Set<PaymentGateway>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<PaymentGateway>> GetAll()
        {
            IEnumerable<PaymentGateway>? entities = await _dbContext.Set<PaymentGateway>().Where(x=>x.IsActive).ToListAsync();
            return entities;
        }

        public async Task<PaymentGateway> Update(PaymentGateway entity)
        {
            _dbContext.Set<PaymentGateway>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
