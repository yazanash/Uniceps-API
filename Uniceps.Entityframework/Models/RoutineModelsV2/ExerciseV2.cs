using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.RoutineModels;

namespace Uniceps.Entityframework.Models.RoutineModelsV2
{
    public class ExerciseV2
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ExerciseId { get; set; } = string.Empty;

        public string NameEn { get; set; } = string.Empty;
        public string NameAr { get; set; } = string.Empty;
        public string MuscleGroupCode { get; set; } = string.Empty;
        public string MuscleHeadCode { get; set; } = string.Empty;
        public string EquipmentCode { get; set; } = string.Empty;
        public string? MuscleAux1 { get; set; }
        public string? MuscleAux2 { get; set; }
        public string? MuscleAux3 { get; set; }
        public string? Implementation { get; set; }
        public int Version { get; set; } = 1;
        public ExerciseMechanism Mechanism { get; set; } 

        public string ImageUrl { get; set; } = string.Empty; 

        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

        [ForeignKey(nameof(MuscleGroupCode))]
        public virtual MuscleGroupV2? MuscleGroupV2 { get; set; }

        [ForeignKey(nameof(MuscleHeadCode))]
        public virtual MuscleHead? MuscleHead { get; set; }

        [ForeignKey(nameof(EquipmentCode))]
        public virtual Equipment? Equipment { get; set; }
    }
    public enum ExerciseMechanism { Bi, Uni, Bodyweight, Alternate }
}
