using Uniceps.Entityframework.Models.NotificationModels;

namespace Uniceps.app.DTOs.UserDeviceDto
{
    public class UserDeviceCreationDto
    {
        public string DeviceToken { get; set; } = "";
        public string DeviceId { get; set; } = "";
        public string Platform { get; set; } = "";
        public string AppVersion { get; set; } = "";
        public string DeviceModel { get; set; } = "";
        public string OsVersion { get; set; } = "";
        public string NotifyToken { get; set; } = "";

        public UserDevice ToModel()
        {
            UserDevice userDevice = new UserDevice();
            userDevice.DeviceToken = DeviceToken;
            userDevice.DeviceId = DeviceId;
            userDevice.Platform = Platform;
            userDevice.AppVersion = AppVersion;
            userDevice.DeviceModel = DeviceModel;
            userDevice.OsVersion = OsVersion;
            userDevice.NotifyToken = NotifyToken;
            return userDevice;
        }
    }
}
