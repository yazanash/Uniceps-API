using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.SystemSubscriptionModels
{
    public class PlanItem
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string? DurationString { get; set; }
        public int DaysCount { get; set; }
        public bool IsFree { get; set; }
        public string? Name { get; set; } = string.Empty;
        public int ProductId { get; set; }
    }
}
