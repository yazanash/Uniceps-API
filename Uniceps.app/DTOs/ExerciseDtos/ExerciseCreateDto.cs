namespace Uniceps.app.DTOs.ExerciseDtos
{
    public class ExerciseCreateDto
    {
        public IFormFile? Image { get; set; }
        public string? Name { get; set; }
        public int MuscleGroupId { get; set; }
    }
}
