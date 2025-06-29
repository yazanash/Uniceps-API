using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Measurements
{
    public class WorkoutLog
    {
        public Guid Id { get; set; }
        public Guid PlayerId { get; set; }
        public int ExerciseId { get; set; }
        public double WeightKg { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public DateTime PerformedAt { get; set; }
    }
}
