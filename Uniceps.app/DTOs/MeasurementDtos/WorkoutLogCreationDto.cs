namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutLogCreationDto
    {
        public string ExerciseId { get; set; } = string.Empty;
        public int ExerciseIndex { get; set; }
        public double Weight { get; set; }
        public int Reps { get; set; }
        public int SetIndex { get; set; }
        public int FinishedReps { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
