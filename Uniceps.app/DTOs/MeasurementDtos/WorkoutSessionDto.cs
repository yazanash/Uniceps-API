using Uniceps.Entityframework.Models.Measurements;

namespace Uniceps.app.DTOs.MeasurementDtos
{
    public class WorkoutSessionDto
    {
        public int Id { get; set; }
        public string Day { get; set; } = "";
        public List<WorkoutLogDto> Logs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public double Progress { get; set; }
    }
}
