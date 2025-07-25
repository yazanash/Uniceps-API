﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Uniceps.Entityframework.Models.SystemSubscriptionModels
{
    public class SystemSubscription: EntityBase
    {
        public string? UserId { get; set; }
        public Guid PlanNID { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsGift { get; set; }
        public string? StripeCheckoutSessionId { get; set; } 
        public bool ISPaid { get; set; }
    }
   
}
