using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public class ProductDataService(AppDbContext dbContext) : IProductDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<Product> Create(Product entity)
        {
            EntityEntry<Product> CreatedResult = await _dbContext.Set<Product>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public Task<bool> Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<Product> Get(int id)
        {
            Product? entity = await _dbContext.Set<Product>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Product>> GetAll()
        {
            IEnumerable<Product>? entities = await _dbContext.Set<Product>().ToListAsync();
            return entities;
        }

        public async Task<Product> GetByAppId(int appId)
        {
            Product? entity = await _dbContext.Set<Product>().AsNoTracking().FirstOrDefaultAsync((e) => e.AppId == appId);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<Product> Update(Product entity)
        {
            _dbContext.Set<Product>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
