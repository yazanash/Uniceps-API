using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.SystemSubscriptionModels
{
    public class PlanModel: EntityBase
    {
        public string? Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInDays { get; set; }
        public PlanTarget TargetUserType { get; set; }
        public bool IsFree { get; set; }
    }
    public enum PlanTarget
    {
        Normal = 0,
        Business = 1
    }
}
