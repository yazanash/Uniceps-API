using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.BusinessLocalModels
{
    public class PlayerModel: EntityBase
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime SubscribeEndDate { get; set; }
        public bool IsTakenContainer { get; set; }
        public bool IsSubscribed { get; set; }
        public double Balance { get; set; }
        public string? BusinessId { get; set; }
        public string? UserId { get; set; }
    }
}
