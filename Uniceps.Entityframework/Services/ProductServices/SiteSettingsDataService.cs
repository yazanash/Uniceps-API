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
    public class SiteSettingsDataService(AppDbContext dbContext) : ISiteSettingsService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<SiteSettings> CreateOrUpdate(SiteSettings entity)
        { 
            SiteSettings? existedEntity = await _dbContext.Set<SiteSettings>().AsNoTracking().FirstOrDefaultAsync();
            if (existedEntity == null)
            {
                EntityEntry<SiteSettings> CreatedResult = await _dbContext.Set<SiteSettings>().AddAsync(entity);
            }
            else
            {
                entity.Id = existedEntity.Id;
                _dbContext.Set<SiteSettings>().Update(entity);
            }
            await _dbContext.SaveChangesAsync();
            return entity;
        }
        public async Task<SiteSettings> Get()
        {
            SiteSettings? entity = await _dbContext.Set<SiteSettings>().AsNoTracking().FirstOrDefaultAsync();
            return entity ?? new SiteSettings();
        }

    }
}
