using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutSessionCreationDto
    {
        public int? ApiId { get; set; }
        public string Day { get; set; } = "";
        public List<WorkoutLogCreationDto> Logs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime FinishedAt { get; set; }
        public double Progress { get; set; }
        public WorkoutSession ToModel()
        {
            WorkoutSession workoutSession = new WorkoutSession()
            {
                Day = Day,
                CreatedAt = CreatedAt,
                FinishedAt = FinishedAt,
                Progress = Progress,
                Id = ApiId ?? 0,
            };
            foreach (WorkoutLogCreationDto workoutLogCreationDto in Logs)
            {
                WorkoutLog workoutLog = new WorkoutLog()
                {
                    ExerciseId = workoutLogCreationDto.ExerciseId,
                    ExerciseIndex = workoutLogCreationDto.ExerciseIndex,
                    WeightKg = workoutLogCreationDto.Weight,
                    Reps = workoutLogCreationDto.Reps,
                    SetIndex = workoutLogCreationDto.SetIndex,
                    CompletedAt = workoutLogCreationDto.CompletedAt,
                    FinishedReps = workoutLogCreationDto.FinishedReps,
                };
                workoutSession.Logs.Add(workoutLog);
            }


            return workoutSession;
        }
    }
}
