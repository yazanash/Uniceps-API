using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.BusinessLocalModels;

namespace Uniceps.Entityframework.Services.BusinessLocalServices
{
    public class BusinessAttendanceRecordDataService(AppDbContext dbContext) : IDataService<BusinessAttendanceRecord>, IUserQueryDataService<BusinessAttendanceRecord>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<BusinessAttendanceRecord> Create(BusinessAttendanceRecord entity)
        {
            EntityEntry<BusinessAttendanceRecord> CreatedResult = await _dbContext.Set<BusinessAttendanceRecord>().AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(Guid id)
        {
            BusinessAttendanceRecord? entity = await _dbContext.Set<BusinessAttendanceRecord>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<BusinessAttendanceRecord>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<BusinessAttendanceRecord> Get(Guid id)
        {
            BusinessAttendanceRecord? entity = await _dbContext.Set<BusinessAttendanceRecord>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<BusinessAttendanceRecord>> GetAll()
        {
            IEnumerable<BusinessAttendanceRecord>? entities = await _dbContext.Set<BusinessAttendanceRecord>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<BusinessAttendanceRecord>> GetAllByUser(string? userid)
        {
            IEnumerable<BusinessAttendanceRecord>? entities = await _dbContext.Set<BusinessAttendanceRecord>()
                .Where(x => x.BusinessId == userid).ToListAsync();
            return entities;
        }

        public async Task<BusinessAttendanceRecord> Update(BusinessAttendanceRecord entity)
        {
            _dbContext.Set<BusinessAttendanceRecord>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
