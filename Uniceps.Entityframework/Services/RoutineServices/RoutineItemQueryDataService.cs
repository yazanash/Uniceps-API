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
    public class RoutineItemQueryDataService : IQueryDataService<RoutineItem>,IEntityQueryDataService<RoutineItem>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineItemQueryDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<RoutineItem> Get(int id)
        {
            RoutineItem? entity = await _contextFactory.Set<RoutineItem>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<RoutineItem>> GetAll()
        {
            IEnumerable<RoutineItem>? entities = await _contextFactory.Set<RoutineItem>().ToListAsync();
            return entities;
        }

        public async Task<IEnumerable<RoutineItem>> GetAllById(int entityId)
        {
            IEnumerable<RoutineItem>? entities = await _contextFactory.Set<RoutineItem>().Where(x => x.DayId == entityId).ToListAsync();
            return entities;
        }
    }
}
