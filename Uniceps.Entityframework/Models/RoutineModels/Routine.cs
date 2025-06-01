using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.RoutineModels
{
    public class Routine
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public virtual List<Day> Days { get; set; } = new List<Day>();
        public DateTime CreatedAt {  get; set; }
        public DateTime UpdatedAt { get; set; }
                 
    }
}
