using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.Measurements
{
    public class WorkoutSession
    {
        public int Id { get; set; }
        public string Day { get; set; } = "";
        public string? UserId { get; set; }
        public List<WorkoutLog> Logs { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime? FinishedAt { get; set; }
        public double Progress { get; set; }
    }
}
