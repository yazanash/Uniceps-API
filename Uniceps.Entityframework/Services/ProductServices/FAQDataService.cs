using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Products;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.ProductServices
{
    public class FAQDataService(AppDbContext dbContext) : IProductRelatedDataService<FrequentlyAskedQuestion>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<FrequentlyAskedQuestion> Create(FrequentlyAskedQuestion entity)
        {
            EntityEntry<FrequentlyAskedQuestion> CreatedResult = await _dbContext.Set<FrequentlyAskedQuestion>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            FrequentlyAskedQuestion? entity = await _dbContext.Set<FrequentlyAskedQuestion>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<FrequentlyAskedQuestion>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<FrequentlyAskedQuestion> Get(int id)
        {
            FrequentlyAskedQuestion? entity = await _dbContext.Set<FrequentlyAskedQuestion>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<FrequentlyAskedQuestion>> GetAllByProductId(int productId)
        {
            IEnumerable<FrequentlyAskedQuestion>? entities = await _dbContext.Set<FrequentlyAskedQuestion>().Where(x => x.ProductId == productId).ToListAsync();
            return entities;
        }
        public async Task<FrequentlyAskedQuestion> Update(FrequentlyAskedQuestion entity)
        {
            _dbContext.Set<FrequentlyAskedQuestion>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
