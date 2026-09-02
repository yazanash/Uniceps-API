using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Measurements;
using Uniceps.Entityframework.Models.NutritionSystem;

namespace Uniceps.Entityframework.Services.NutritionServices
{
    public class DietLogDataService(AppDbContext dbContext) : IDietLogDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<IEnumerable<DietLog>> GetAllByUserAsync(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Enumerable.Empty<DietLog>();

            return await _dbContext.Set<DietLog>()
                .AsNoTracking()
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();
        }

        public async Task<DietLog> UpsertAsync(DietLog dietLog)
        {
            var existingSession = await _dbContext.Set<DietLog>()
        .FirstOrDefaultAsync(s => s.Id == dietLog.Id && s.UserId == dietLog.UserId);

            if (existingSession == null)
            {
                dietLog.Id = 0;
                var createdResult = await _dbContext.Set<DietLog>().AddAsync(dietLog);
                await _dbContext.SaveChangesAsync();
                return createdResult.Entity;
            }

            _dbContext.Entry(existingSession).CurrentValues.SetValues(dietLog);
            await _dbContext.SaveChangesAsync();


            return existingSession;
        }
    }
}
