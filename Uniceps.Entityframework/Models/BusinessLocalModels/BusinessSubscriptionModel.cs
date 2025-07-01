using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.BusinessLocalModels
{
    public class BusinessSubscriptionModel: EntityBase
    {

        public Guid ServiceNID { get; set; }
        [ForeignKey("ServiceNID")]
        public virtual BusinessServiceModel? BusinessService { get; set; }
        public string? BusinessId { get; set; }
        public Guid PlayerId { get; set; }
        [ForeignKey("PlayerId")]
        public virtual PlayerModel? PlayerModel { get; set; }
        public DateTime RollDate { get; set; }
        public double Price { get; set; }
        public double OfferValue { get; set; }
        public string? OfferDes { get; set; }
        public double PriceAfterOffer { get; set; }
        public int SessionCount { get; set; }
        public bool IsStopped { get; set; }
        public double PaidValue { get; set; }
        public double RestValue { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastPaid { get; set; }
        public virtual List<BusinessPaymentModel> BusinessPaymentModels { get; set; } = new List<BusinessPaymentModel>();
    }
}
