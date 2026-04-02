using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;
using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.Entityframework.Services.ExerciseServices
{
    public interface IExerciseV2DataService
    {
        Task<IEnumerable<ExerciseV2>> GetAllExercisesAsync(ExerciseFilter filter);
        Task<IEnumerable<MuscleGroupV2>> GetMuscleGroupsAsync();
        Task<IEnumerable<Equipment>> GetEquipmentssAsync();
        Task SyncMuscleGroupsAsync(List<MuscleGroupV2> groups);
        Task SyncEquipmentsAsync(List<Equipment> equipments);
        Task SyncExercisesAsync(List<ExerciseV2> exercises);
        Task SyncMuscleHeads( List<MuscleHead> heads);
        Task<string> GetExerciseImage(string exerciseId);
    }
}
