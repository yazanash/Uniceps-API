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
    public class WorkoutSessionDataService(AppDbContext dbContext) : IIntDataService<WorkoutSession>, IUserQueryDataService<WorkoutSession>
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<WorkoutSession> Create(WorkoutSession entity)
        {
            EntityEntry<WorkoutSession> CreatedResult = await _dbContext.Set<WorkoutSession>().AddAsync(entity);

            await _dbContext.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            WorkoutSession? entity = await _dbContext.Set<WorkoutSession>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<WorkoutSession>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<WorkoutSession> Get(int id)
        {
            WorkoutSession? entity = await _dbContext.Set<WorkoutSession>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<WorkoutSession>> GetAll()
        {
            IEnumerable<WorkoutSession>? entities = await _dbContext.Set<WorkoutSession>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<WorkoutSession>> GetAllByUser(string? userid)
        {
            IEnumerable<WorkoutSession>? entities = await _dbContext.Set<WorkoutSession>().Where(x => x.UserId == userid).Include(x=>x.Logs).ToListAsync();
            return entities;
        }

        public async Task<WorkoutSession> Update(WorkoutSession entity)
        {
            _dbContext.Set<WorkoutSession>().Update(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
