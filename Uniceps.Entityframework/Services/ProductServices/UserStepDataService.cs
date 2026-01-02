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
using Uniceps.Entityframework.Models.Products;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public class UserStepDataService(AppDbContext dbContext) : IProductRelatedDataService<UserStep>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<UserStep> Create(UserStep entity)
        {
            EntityEntry<UserStep> CreatedResult = await _dbContext.Set<UserStep>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            UserStep? entity = await _dbContext.Set<UserStep>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<UserStep>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserStep> Get(int id)
        {
            UserStep? entity = await _dbContext.Set<UserStep>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }
        public async Task<IEnumerable<UserStep>> GetAllByProductId(int productId)
        {
            IEnumerable<UserStep>? entities = await _dbContext.Set<UserStep>().Where(x => x.ProductId == productId).ToListAsync();
            return entities;
        }

        public async Task<UserStep> Update(UserStep entity)
        {
            _dbContext.Set<UserStep>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
