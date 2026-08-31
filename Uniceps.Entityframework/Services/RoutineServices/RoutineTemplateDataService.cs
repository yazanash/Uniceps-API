using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Billing;
using Uniceps.Entityframework.Models.NutritionSystem;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.Entityframework.Services.RoutineServices
{
    public class RoutineTemplateDataService(AppDbContext dbContext) : IRoutineTemplateDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<RoutineTemplate> CreateAsync(RoutineTemplate entity)
        {
            EntityEntry<RoutineTemplate> CreatedResult = await _dbContext.Set<RoutineTemplate>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> DeactiveAsync(Guid id)
        {
            RoutineTemplate? entity = await _dbContext.Set<RoutineTemplate>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            entity.IsActive = false;
            _dbContext.Set<RoutineTemplate>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            RoutineTemplate? entity = await _dbContext.Set<RoutineTemplate>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<RoutineTemplate>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<RoutineTemplate> Get(Guid id)
        {
            RoutineTemplate? entity = await _dbContext.Set<RoutineTemplate>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<RoutineTemplate>> GetAllByQuery(TargetGender? targetGender,TargetLanguage? targetLanguage)
        {
            IQueryable<RoutineTemplate> query = _dbContext.Set<RoutineTemplate>();

            if (targetGender.HasValue)
            {
                query = query.Where(x => x.TargetGender == targetGender || x.TargetGender== TargetGender.Both);
            }
            if (targetLanguage.HasValue)
            {
                query = query.Where(x => x.TargetLanguage == targetLanguage);
            }
            return await query.AsNoTracking()
                        .ToListAsync();
        }

        public async Task<RoutineTemplate> UpdateAsync(RoutineTemplate entity)
        {
            _dbContext.Set<RoutineTemplate>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
