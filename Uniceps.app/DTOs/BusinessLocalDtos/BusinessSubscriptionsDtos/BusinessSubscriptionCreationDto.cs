﻿namespace Uniceps.app.DTOs.BusinessLocalDtos.BusinessSubscriptionsDtos
{
    public class BusinessSubscriptionCreationDto
    {
        public Guid ServiceId { get; set; }
        public Guid PlayerId { get; set; }
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
    }
}
