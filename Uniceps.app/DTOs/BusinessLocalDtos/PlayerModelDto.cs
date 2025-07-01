namespace Uniceps.app.DTOs.BusinessLocalDtos
{
    public class PlayerModelDto
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Phone { get; set; }
        public DateTime SubscribeDate { get; set; }
        public DateTime SubscribeEndDate { get; set; }
        public bool IsTakenContainer { get; set; }
        public bool IsSubscribed { get; set; }
        public double Balance { get; set; }
        public string? BusinessName{ get; set; }
        public string? UserId { get; set; }
    }
}
