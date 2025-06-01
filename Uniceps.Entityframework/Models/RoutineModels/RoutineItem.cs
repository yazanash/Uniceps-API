using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class RoutineItem
    {
        public int Id { get; set; }
        public int ExerciseId { get; set; }
        public Exercise Exercise{ get; set; }
        public int DayId { get; set; }
        public Day Day { get; set; } 
        public int Order { get; set; }
        public virtual List<ItemSet> Sets { get; set; } = new List<ItemSet>();
    }
}
