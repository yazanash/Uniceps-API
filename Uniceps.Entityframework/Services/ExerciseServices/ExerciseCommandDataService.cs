using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Internal;
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
    public class ExerciseCommandDataService : ICommandDataService<Exercise>
    {
        private readonly AppDbContext _contextFactory;

        public ExerciseCommandDataService(AppDbContext contextFactory)
        {
            _contextFactory = contextFactory;
        }
        public async Task<Exercise> Create(Exercise entity)
        {
            EntityEntry<Exercise> CreatedResult = await _contextFactory.Set<Exercise>().AddAsync(entity);
            await _contextFactory.SaveChangesAsync();
            return CreatedResult.Entity;
        }

        public async Task<bool> Delete(int id)
        {
            Exercise? entity = await _contextFactory.Set<Exercise>().FirstOrDefaultAsync((e) => e.Id == id);
            if (entity == null)
                throw new Exception();
            _contextFactory.Set<Exercise>().Remove(entity!);
            await _contextFactory.SaveChangesAsync();
            return true;
        }



        public async Task<Exercise> Update(Exercise entity)
        {
            _contextFactory.Set<Exercise>().Update(entity);
            await _contextFactory.SaveChangesAsync();
            return entity;
        }
    }
}
