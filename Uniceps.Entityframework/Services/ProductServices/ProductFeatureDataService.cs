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

namespace Uniceps.Entityframework.Services.ProductServices
{
    public class ProductFeatureDataService(AppDbContext dbContext) : IProductRelatedDataService<ProductFeature>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<ProductFeature> Create(ProductFeature entity)
        {
            EntityEntry<ProductFeature> CreatedResult = await _dbContext.Set<ProductFeature>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            ProductFeature? entity = await _dbContext.Set<ProductFeature>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<ProductFeature>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<ProductFeature> Get(int id)
        {
            ProductFeature? entity = await _dbContext.Set<ProductFeature>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<ProductFeature>> GetAllByProductId(int productId)
        {
            IEnumerable<ProductFeature>? entities = await _dbContext.Set<ProductFeature>().Where(x => x.ProductId == productId).ToListAsync();
            return entities;
        }

        public async Task<ProductFeature> Update(ProductFeature entity)
        {
            _dbContext.Set<ProductFeature>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
