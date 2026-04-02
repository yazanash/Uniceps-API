namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutLogDto
    {
        public int Id { get; set; }
        public int SessionId { get; set; }
        public string ExerciseId { get; set; } = string.Empty;
        public int ExerciseIndex { get; set; }
        public double WeightKg { get; set; }
        public int Reps { get; set; }
        public int SetIndex { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
