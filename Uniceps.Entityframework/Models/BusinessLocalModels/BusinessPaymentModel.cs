using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.BusinessLocalModels
{
    public class BusinessPaymentModel:EntityBase
    {
        public Guid BusinessSubscriptionNID { get; set; }
        [ForeignKey("BusinessSubscriptionNID")]
        public virtual BusinessSubscriptionModel? BusinessSubscription { get; set; }
        public double Amount { get; set; }
        public string? Description { get; set; }
        public DateTime IssueDate { get; set; }
        public string? BusinessId { get; set; }
    }
}
