using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.Entityframework.Services.ExerciseServices
{
    public class ExerciseV2DataService(AppDbContext dbContext) : IExerciseV2DataService
    {
        private readonly AppDbContext _dbContext = dbContext;

        public async Task<IEnumerable<ExerciseV2>> GetAllExercisesAsync(ExerciseFilter filter)
        {
            IQueryable<ExerciseV2> query = _dbContext.Set<ExerciseV2>().Include(x=>x.Equipment).Include(x=>x.MuscleGroupV2).Include(x=>x.MuscleHead).AsNoTracking();

            if (filter == null) return await query.ToListAsync();

            if (!string.IsNullOrEmpty(filter.MuscleGroupCode))
                query = query.Where(e => e.MuscleGroupCode == filter.MuscleGroupCode);

            if (!string.IsNullOrEmpty(filter.MuscleHeadCode))
                query = query.Where(e => e.MuscleHeadCode == filter.MuscleHeadCode);

            if (!string.IsNullOrEmpty(filter.EquipmentCode))
                query = query.Where(e => e.EquipmentCode == filter.EquipmentCode);

            if (filter.Mechanism.HasValue)
                query = query.Where(e => e.Mechanism == filter.Mechanism.Value);

            if (!string.IsNullOrEmpty(filter.SearchTerm))
                query = query.Where(e => e.NameEn.Contains(filter.SearchTerm) || e.NameAr.Contains(filter.SearchTerm));

            if (filter.LastSync.HasValue)
            {
                var safeSyncTime = filter.LastSync.Value.AddMinutes(5);
                query = query.Where(e => e.LastUpdated > safeSyncTime);
            }

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Equipment>> GetEquipmentssAsync()
        {
            IEnumerable<Equipment>? entities = await _dbContext.Set<Equipment>().ToListAsync();
            return entities;
        }

        public async Task<string> GetExerciseImage(string exerciseId)
        {
            var exisitingExercise = await _dbContext.Set<ExerciseV2>()
                   .FirstOrDefaultAsync(g => g.ExerciseId == exerciseId);

            return exisitingExercise?.ImageUrl??"";
        }

        public async Task<IEnumerable<MuscleGroupV2>> GetMuscleGroupsAsync()
        {
            IEnumerable<MuscleGroupV2>? entities = await _dbContext.Set<MuscleGroupV2>().Include(x=>x.Heads).ToListAsync();
            return entities;
        }

        public async Task SyncEquipmentsAsync(List<Equipment> equipments)
        {
            foreach (var equipment in equipments)
            {
                var existingEquipment = await _dbContext.Set<Equipment>()
                    .FirstOrDefaultAsync(g => g.Code == equipment.Code);

                if (existingEquipment == null)
                {
                    _dbContext.Set<Equipment>().Add(equipment);
                }
                else
                {
                    _dbContext.Entry(existingEquipment).CurrentValues.SetValues(equipment);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SyncExercisesAsync(List<ExerciseV2> exercises)
        {
            foreach (var exercise in exercises)
            {
                var exisitingExercise = await _dbContext.Set<ExerciseV2>()
                    .FirstOrDefaultAsync(g => g.ExerciseId == exercise.ExerciseId);

                if (exisitingExercise == null)
                {
                    _dbContext.Set<ExerciseV2>().Add(exercise);
                }
                else
                {
                    _dbContext.Entry(exisitingExercise).CurrentValues.SetValues(exercise);
                }
            }
            await _dbContext.SaveChangesAsync();
        }

        public async Task SyncMuscleGroupsAsync(List<MuscleGroupV2> groups)
        {
            foreach (var group in groups)
            {
                var existingGroup = await _dbContext.Set<MuscleGroupV2>()
                    .Include(g => g.Heads)
                    .FirstOrDefaultAsync(g => g.Code == group.Code);

                if (existingGroup == null)
                {
                    _dbContext.Set<MuscleGroupV2>().Add(group);
                }
                else
                {
                    _dbContext.Entry(existingGroup).CurrentValues.SetValues(group);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
        public async Task SyncMuscleHeads(List<MuscleHead> heads)
        {
            foreach (var head in heads)
            {
                var existingHead = await _dbContext.Set<MuscleHead>()
                    .FirstOrDefaultAsync(g => g.Code == head.Code && g.MuscleGroupCode == head.MuscleGroupCode);

                if (existingHead == null)
                {
                    _dbContext.Set<MuscleHead>().Add(head);
                }
                else
                {
                    _dbContext.Entry(existingHead).CurrentValues.SetValues(head);
                }
            }
            await _dbContext.SaveChangesAsync();
        }
    }
}
