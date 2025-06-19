namespace Uniceps.app.DTOs.UserDeviceDto
{
    public class UserDeviceCreationDto
    {
        public string DeviceToken { get; set; } = null!;
        public string DeviceId { get; set; } = null!;
        public string Platform { get; set; } = null!;
        public string AppVersion { get; set; } = null!;
        public string DeviceModel { get; set; } = null!;
        public string OsVersion { get; set; } = null!;
    }
}
