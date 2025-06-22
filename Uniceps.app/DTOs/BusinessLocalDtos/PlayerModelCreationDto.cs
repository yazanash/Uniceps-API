namespace Uniceps.app.DTOs.BusinessLocalDtos
{
    public class PlayerModelCreationDto
    {
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime SubscribeEndDate { get; set; }
        public bool IsSubscribed { get; set; }
        public double Balance { get; set; }
        public string? UserId { get; set; }
    }
}
