using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.MuscleGroupServices
{
    public class MuscleGroupQueryDataService : IQueryDataService<MuscleGroup>
    {
        private readonly AppDbContext _contextFactory;

        public MuscleGroupQueryDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<MuscleGroup> Get(int id)
        {
            MuscleGroup? entity = await _contextFactory.Set<MuscleGroup>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<MuscleGroup>> GetAll()
        {
            IEnumerable<MuscleGroup>? entities = await _contextFactory.Set<MuscleGroup>().ToListAsync();
            return entities;

        }

    }
}
