using Uniceps.app.DTOs.SystemSubscriptionDtos;
using Uniceps.app.DTOs.UserDeviceDto;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Extensions.UserDeviceMappers
{
    public class UserDeviceMapperExtension : IMapperExtension<UserDevice, UserDeviceDto, UserDeviceCreationDto>
    {
        public UserDevice FromCreationDto(UserDeviceCreationDto data)
        {
            UserDevice userDevice = new UserDevice();

            userDevice.DeviceToken = data.DeviceToken;
            userDevice.DeviceId = data.DeviceId;
            userDevice.Platform = data.Platform;
            userDevice.AppVersion = data.AppVersion;
            userDevice.DeviceModel = data.DeviceModel;
            userDevice.OsVersion = data.OsVersion;
            return userDevice;
        }

        public UserDeviceDto ToDto(UserDevice data)
        {
            UserDeviceDto userDeviceDto = new UserDeviceDto();
            userDeviceDto.Id = data.Id;
            userDeviceDto.DeviceToken = data.DeviceToken;
            userDeviceDto.DeviceId = data.DeviceId;
            userDeviceDto.Platform = data.Platform;
            userDeviceDto.AppVersion = data.AppVersion;
            userDeviceDto.DeviceModel = data.DeviceModel;
            userDeviceDto.OsVersion = data.OsVersion;
            userDeviceDto.RegisteredAt = data.RegisteredAt;
            userDeviceDto.LastSeen = data.LastSeen;
            userDeviceDto.IsActive = data.IsActive;
            return userDeviceDto;
        }
    }
}
