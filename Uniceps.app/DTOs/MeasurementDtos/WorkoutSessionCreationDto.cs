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
    }
}
