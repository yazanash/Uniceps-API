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
        public PlanTarget TargetUserType { get; set; }
        public int ProductId { get; set; }
        public List<PlanItem> PlanItems { get; set; } = new();
    }
    public enum PlanTarget
    {
        Normal = 0,
        Business = 1
    }
}
