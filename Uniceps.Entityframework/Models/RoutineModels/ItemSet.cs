using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class ItemSet: EntityBase
    {
        public int RoundIndex { get; set; }
        public int Repetition { get; set; }
        public Guid RoutineItemNID { get; set; } 
        public RoutineItem? RoutineItem { get; set; } 
    }
}
