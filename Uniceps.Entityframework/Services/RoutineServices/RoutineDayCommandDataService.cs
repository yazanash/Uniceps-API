using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.RoutineServices
{
    public class RoutineDayCommandDataService : ICommandDataService<Day>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineDayCommandDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Day> Create(Day entity)
        {
            EntityEntry<Day> CreatedResult = await _contextFactory.Set<Day>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            Day? entity = await _contextFactory.Set<Day>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<Day>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }

        public async Task<Day> Update(Day entity)
        {
            _contextFactory.Set<Day>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
