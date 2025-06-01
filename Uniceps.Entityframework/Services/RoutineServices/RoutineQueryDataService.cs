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
    public class RoutineQueryDataService : IQueryDataService<Routine>
    {
        private readonly AppDbContext _contextFactory;

        public RoutineQueryDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Routine> Get(int id)
        {
            Routine? entity = await _contextFactory.Set<Routine>().AsNoTracking()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Routine>> GetAll()
        {
            IEnumerable<Routine>? entities = await _contextFactory.Set<Routine>()
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(x => x.Exercise)
                .Include(x => x.Days)
                .ThenInclude(d => d.RoutineItems)
                .ThenInclude(i => i.Sets).ToListAsync();
            return entities;

        }
    }
}
