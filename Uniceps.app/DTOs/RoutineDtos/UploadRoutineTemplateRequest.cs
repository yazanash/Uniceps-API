using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class UploadRoutineTemplateRequest
    {
        public required IFormFile File { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public TargetGender TargetGender { get; set; }
        public TargetLanguage TargetLanguage { get; set; }
        public RoutineLevel Level { get; set; }
    }
}
