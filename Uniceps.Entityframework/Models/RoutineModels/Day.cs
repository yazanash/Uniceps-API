using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class Day
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public int RoutineId { get; set; }
        public Routine? Routine{ get; set; }
        public List<RoutineItem> RoutineItems { get; set; } = new List<RoutineItem>();
    }
}
