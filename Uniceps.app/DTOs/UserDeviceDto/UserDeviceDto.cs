namespace Uniceps.app.DTOs.UserDeviceDto
{
    public class UserDeviceDto
    {
        public Guid Id { get; set; }
        public string? DeviceToken { get; set; } 
        public string? DeviceId { get; set; } 
        public string? Platform { get; set; }
        public string? AppVersion { get; set; }
        public string? DeviceModel { get; set; } 
        public string? OsVersion { get; set; }
        public string? NotifyToken { get; set; }
        public DateTime RegisteredAt { get; set; }
        public DateTime LastSeen { get; set; } 
        public bool IsActive { get; set; }
    }
}
