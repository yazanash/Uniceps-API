using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.Entityframework.Services.MeasurementServices
{
    public class WorkoutSessionDataService(AppDbContext dbContext) : IWorkoutSessionService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<WorkoutSession> UpsertAsync(WorkoutSession entity)
        {
            var existingSession = await _dbContext.WorkoutSessions
         .FirstOrDefaultAsync(s => s.Id == entity.Id && s.UserId == entity.UserId);

            if (existingSession == null)
            {
                entity.Id = 0;
                var createdResult = await _dbContext.WorkoutSessions.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return createdResult.Entity;
            }

            _dbContext.Entry(existingSession).CurrentValues.SetValues(entity);
            await _dbContext.SaveChangesAsync();

            await SyncLogsAsync(existingSession.Id, entity.Logs);

            return existingSession;
        }
        public async Task<IEnumerable<WorkoutSession>> GetAllByUser(string? userid)
        {
            if (string.IsNullOrEmpty(userid))
                return Enumerable.Empty<WorkoutSession>();

            return await _dbContext.WorkoutSessions
                .AsNoTracking()
                .Where(x => x.UserId == userid)
                .Include(x => x.Logs)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }
        private async Task SyncLogsAsync(int sessionId, ICollection<WorkoutLog> incomingLogs)
        {
            var safeIncomingLogs = incomingLogs ?? new List<WorkoutLog>();

            var dbLogs = await _dbContext.WorkoutLogs
                .Where(l => l.WorkoutSessionId == sessionId)
                .ToListAsync();

            var incomingKeys = safeIncomingLogs
                .Select(l => (
                    ExerciseId: l.ExerciseId?.Trim().ToLowerInvariant() ?? string.Empty,
                    l.ExerciseIndex,
                    l.SetIndex
                ))
                .ToHashSet();

            var logsToRemove = dbLogs
                .Where(dbLog => !incomingKeys.Contains((
                    dbLog.ExerciseId?.Trim().ToLowerInvariant() ?? string.Empty,
                    dbLog.ExerciseIndex,
                    dbLog.SetIndex
                )))
                .ToList();

            if (logsToRemove.Any())
            {
                _dbContext.WorkoutLogs.RemoveRange(logsToRemove);
            }

            foreach (var incomingLog in safeIncomingLogs)
            {
                var incomingKeyStr = incomingLog.ExerciseId?.Trim().ToLowerInvariant() ?? string.Empty;

                var existingLog = dbLogs.FirstOrDefault(l =>
                    (l.ExerciseId?.Trim().ToLowerInvariant() ?? string.Empty) == incomingKeyStr &&
                    l.ExerciseIndex == incomingLog.ExerciseIndex &&
                    l.SetIndex == incomingLog.SetIndex);

                if (existingLog != null)
                {
                    _dbContext.Entry(existingLog).CurrentValues.SetValues(new
                    {
                        incomingLog.WeightKg,
                        incomingLog.Reps,
                        incomingLog.FinishedReps,
                        incomingLog.CompletedAt
                    });
                }
                else
                {
                    incomingLog.Id = 0; 
                    incomingLog.WorkoutSessionId = sessionId;
                    await _dbContext.WorkoutLogs.AddAsync(incomingLog);
                }
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
