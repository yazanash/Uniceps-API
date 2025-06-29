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
    public class BodyMeasurementDataService : IDataService<BodyMeasurement>
    {
        private readonly AppDbContext _contextFactory;

        public BodyMeasurementDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<BodyMeasurement> Create(BodyMeasurement entity)
        {
            EntityEntry<BodyMeasurement> CreatedResult = await _contextFactory.Set<BodyMeasurement>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BodyMeasurement? entity = await _contextFactory.Set<BodyMeasurement>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<BodyMeasurement>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<BodyMeasurement> Get(Guid id)
        {
            BodyMeasurement? entity = await _contextFactory.Set<BodyMeasurement>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BodyMeasurement>> GetAll()
        {
            IEnumerable<BodyMeasurement>? entities = await _contextFactory.Set<BodyMeasurement>().ToListAsync();
            return entities;
        }

     

        public async Task<BodyMeasurement> Update(BodyMeasurement entity)
        {
            _contextFactory.Set<BodyMeasurement>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
