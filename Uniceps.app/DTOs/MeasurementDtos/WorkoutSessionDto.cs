using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutSessionDto
    {
        public WorkoutSessionDto(WorkoutSession data)
        {
            ApiId = data.Id;
            Day = data.Day;
            CreatedAt = data.CreatedAt;
            FinishedAt = data.FinishedAt;
            Progress = data.Progress;
            foreach (WorkoutLog workoutLog in data.Logs)
            {
                WorkoutLogDto workoutLogDto = new WorkoutLogDto()
                {
                    ApiId = workoutLog.Id,
                    SessionId = workoutLog.WorkoutSessionId,
                    ExerciseId = workoutLog.ExerciseId,
                    ExerciseIndex = workoutLog.ExerciseIndex,
                    Weight = workoutLog.WeightKg,
                    Reps = workoutLog.Reps,
                    FinishedReps = workoutLog.FinishedReps,
                    SetIndex = workoutLog.SetIndex,
                    CompletedAt = workoutLog.CompletedAt,
                };
                Logs.Add(workoutLogDto);
            }
        }
        public int ApiId { get; set; }
        public string Day { get; set; } = "";
        public List<WorkoutLogDto> Logs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public double Progress { get; set; }
    }
}
