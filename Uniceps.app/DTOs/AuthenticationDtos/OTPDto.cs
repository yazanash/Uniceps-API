namespace Uniceps.app.DTOs.AuthenticationDtos
{
    public class OTPDto
    {
        public string? Email { get; set; }
        public int Otp { get; set; }
        public string DeviceToken { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string Platform { get; set; } = "";
        public string AppVersion { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string OsVersion { get; set; } = "";
        public string NotifyToken { get; set; } = "";
    }
}
