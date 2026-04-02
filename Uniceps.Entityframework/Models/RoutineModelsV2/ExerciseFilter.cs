using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModelsV2
{
    public class ExerciseFilter
    {
        public string? MuscleGroupCode { get; set; }
        public string? MuscleHeadCode { get; set; }
        public string? EquipmentCode { get; set; }
        public ExerciseMechanism? Mechanism { get; set; }
        public string? SearchTerm { get; set; }
    }
}
