using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.ExerciseServices
{
    public class ExerciseQueryDataService : IQueryDataService<Exercise>, IEntityQueryDataService<Exercise>
    {
        private readonly AppDbContext _contextFactory;

        public ExerciseQueryDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Exercise> Get(int id)
        {
            Exercise? entity = await _contextFactory.Set<Exercise>().AsNoTracking().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }

        public async Task<IEnumerable<Exercise>> GetAll()
        {
            IEnumerable<Exercise>? entities = await _contextFactory.Set<Exercise>().ToListAsync();
            return entities;

        }

        public async Task<IEnumerable<Exercise>> GetAllById(int entityId)
        {
            IEnumerable<Exercise>? entities = await _contextFactory.Set<Exercise>().Where(x=>x.MuscleGroupId==entityId).ToListAsync();
            return entities;
        }
    }
}
