using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class Day: EntityBase
    {
        public string? Name { get; set; }
        public Guid RoutineNID { get; set; }
        public Routine? Routine{ get; set; }
        public List<RoutineItem> RoutineItems { get; set; } = new List<RoutineItem>();
    }
}
