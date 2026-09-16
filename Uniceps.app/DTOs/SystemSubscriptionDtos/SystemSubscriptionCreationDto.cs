namespace Uniceps.app.DTOs.SystemSubscriptionDtos
{
    public class SystemSubscriptionCreationDto
    {
        public int PlanItemId { get; set; }
        public string Email { get; set; } = string.Empty;   
    }
}
