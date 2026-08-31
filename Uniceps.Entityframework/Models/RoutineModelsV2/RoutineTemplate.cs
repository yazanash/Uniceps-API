using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModelsV2
{
    public class RoutineTemplate
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string Description { get; set; } = string.Empty;
        public RoutineLevel Level { get; set; }
        public TargetGender TargetGender { get; set; }
        public TargetLanguage TargetLanguage { get; set; }
        public int DaysCount { get; set; }
        public bool IsActive { get; set; } = true;
        public string PayloadJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
    public enum RoutineLevel
    {
        None = 0,
        Beginner = 1,
        Novice = 2,
        Intermediate = 3,
        Advanced = 4,
        Elite = 5
    }
    public enum TargetGender
    {
        Both = 0,
        Male = 1,
        Female = 2,
    }
    public enum TargetLanguage
    {
        English = 1,
        Arabic = 2
    }
}
