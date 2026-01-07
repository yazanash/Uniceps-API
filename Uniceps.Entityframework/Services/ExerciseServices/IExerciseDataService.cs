using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Services.ExerciseServices
{
    public interface IExerciseDataService
    {
        Task<Exercise> UpsertAsync(Exercise entity);
        Task<bool> Delete(int id);
        Task<Exercise> Get(int id);
        Task<IEnumerable<Exercise>> GetAll();
        Task<IEnumerable<Exercise>> GetAllById(int entityId);
    }
}
