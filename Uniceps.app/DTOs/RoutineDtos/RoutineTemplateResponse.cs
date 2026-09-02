using Uniceps.Entityframework.Models.RoutineModelsV2;

namespace Uniceps.app.DTOs.RoutineDtos
{
    public class RoutineTemplateResponse
    {
        private RoutineTemplate RoutineTemplate;
        public RoutineTemplateResponse(RoutineTemplate routineTemplate)
        {
            RoutineTemplate = routineTemplate;
        }
        public string ApiId => RoutineTemplate.Id.ToString();
        public string? Title => RoutineTemplate.Title;
        public string? Description=> RoutineTemplate.Description;
        public RoutineLevel Level => RoutineTemplate.Level;
        public TargetGender TargetGender => RoutineTemplate.TargetGender;
        public int DaysCount => RoutineTemplate.DaysCount;
        public TargetLanguage TargetLanguage => RoutineTemplate.TargetLanguage;
    }
}
