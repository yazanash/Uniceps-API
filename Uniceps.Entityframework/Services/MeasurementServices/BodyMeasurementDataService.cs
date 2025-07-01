using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.Entityframework.Services.MeasurementServices
{
    public class BodyMeasurementDataService(AppDbContext dbContext) : IDataService<BodyMeasurement>, IUserQueryDataService<BodyMeasurement>
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<BodyMeasurement> Create(BodyMeasurement entity)
        {
            EntityEntry<BodyMeasurement> CreatedResult = await _dbContext.Set<BodyMeasurement>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BodyMeasurement? entity = await _dbContext.Set<BodyMeasurement>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BodyMeasurement>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BodyMeasurement> Get(Guid id)
        {
            BodyMeasurement? entity = await _dbContext.Set<BodyMeasurement>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BodyMeasurement>> GetAll()
        {
            IEnumerable<BodyMeasurement>? entities = await _dbContext.Set<BodyMeasurement>().ToListAsync();
            return entities;
        }
        public async Task<IEnumerable<BodyMeasurement>> GetAllByUser(string? userid)
        {
            IEnumerable<BodyMeasurement>? entities = await _dbContext.Set<BodyMeasurement>().Include(x => x.PlayerModel).Where(x => x.PlayerModel != null && x.PlayerModel.UserId == userid).ToListAsync();
            return entities;
        }

        public async Task<BodyMeasurement> Update(BodyMeasurement entity)
        {
            _dbContext.Set<BodyMeasurement>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
