namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class MembershipDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public string? Plan { get; set; }
        public decimal Price { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
