using Microsoft.EntityFrameworkCore;
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
    public class RoutineDayQueryDataService : IQueryDataService<Day>,IEntityQueryDataService<Day>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineDayQueryDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Day> Get(int id)
        {
            Day? entity = await _contextFactory.Set<Day>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Day>> GetAll()
        {
            IEnumerable<Day>? entities = await _contextFactory.Set<Day>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<Day>> GetAllById(int entityId)
        {
            IEnumerable<Day>? entities = await _contextFactory.Set<Day>().Where(x=>x.RoutineId==entityId).ToListAsync();
            return entities;
        }
    }
}
