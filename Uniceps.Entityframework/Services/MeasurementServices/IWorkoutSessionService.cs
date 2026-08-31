using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.Entityframework.Services.MeasurementServices
{
    public interface IWorkoutSessionService
    {
        Task<WorkoutSession> UpsertAsync(WorkoutSession entity);
        Task<IEnumerable<WorkoutSession>> GetAllByUser(string? userid);

    }
}
