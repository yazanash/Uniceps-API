using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class Exercise
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(255)]
        public string? Name { get; set; }

        [ForeignKey(nameof(MuscleGroup))]
        public int MuscleGroupId { get; set; }
        public MuscleGroup? MuscleGroup { get; set; }
        public string? ImageUrl { get; set; }
    }
}
