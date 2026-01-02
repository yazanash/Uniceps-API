namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutLogCreationDto
    {
        public int ExerciseId { get; set; }
        public int ExerciseIndex { get; set; }
        public double WeightKg { get; set; }
        public int Reps { get; set; }
        public int SetIndex { get; set; }
        public DateTime CompletedAt { get; set; }
    }
}
