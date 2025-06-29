namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class SystemSubscriptionDto
    {
        public Guid Id { get; set; }
        public string? Plan { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }
        public bool IsGift { get; set; }
    }
}
