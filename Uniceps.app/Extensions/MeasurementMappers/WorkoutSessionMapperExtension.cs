using Uniceps.app.DTOs.MeasurementDtos;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.Extensions.MeasurementMappers
{
    public class WorkoutSessionMapperExtension : IMapperExtension<WorkoutSession, WorkoutSessionDto, WorkoutSessionCreationDto>
    {
        public WorkoutSession FromCreationDto(WorkoutSessionCreationDto data)
        {
            WorkoutSession workoutSession = new WorkoutSession()
            {
                Day = data.Day,
                CreatedAt = data.CreatedAt,
                FinishedAt = data.FinishedAt,
                Progress = data.Progress
            };
            foreach (WorkoutLogCreationDto workoutLogCreationDto in data.Logs)
            {
                WorkoutLog workoutLog = new WorkoutLog()
                {
                    ExerciseId = workoutLogCreationDto.ExerciseId,
                    ExerciseIndex = workoutLogCreationDto.ExerciseIndex,
                    WeightKg = workoutLogCreationDto.WeightKg,
                    Reps = workoutLogCreationDto.Reps,
                    SetIndex = workoutLogCreationDto.SetIndex,
                    CompletedAt = workoutLogCreationDto.CompletedAt,
                };
                workoutSession.Logs.Add(workoutLog);
            }


            return workoutSession;
        }

        public WorkoutSessionDto ToDto(WorkoutSession data)
        {
            WorkoutSessionDto workoutSessionDto = new WorkoutSessionDto()
            {
                Id = data.Id,
                Day = data.Day,
                CreatedAt = data.CreatedAt,
                FinishedAt = data.FinishedAt,
                Progress = data.Progress
            };
            foreach (WorkoutLog workoutLog in data.Logs)
            {
                WorkoutLogDto workoutLogDto = new WorkoutLogDto()
                {
                    Id = workoutLog.Id,
                    SessionId = workoutLog.SessionId,
                    ExerciseId = workoutLog.ExerciseId,
                    ExerciseIndex = workoutLog.ExerciseIndex,
                    WeightKg = workoutLog.WeightKg,
                    Reps = workoutLog.Reps,
                    SetIndex = workoutLog.SetIndex,
                    CompletedAt = workoutLog.CompletedAt,
                };
                workoutSessionDto.Logs.Add(workoutLogDto);
            }

            return workoutSessionDto;
        }
    }
}
