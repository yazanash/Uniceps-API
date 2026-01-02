using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.DBContext;
using Uniceps.Entityframework.Models.NotificationModels;
using Uniceps.Entityframework.Models.Profile;

namespace Uniceps.Entityframework.Services.NotificationSystemServices
{
    public class UserDeviceDataService(AppDbContext dbContext) : IUserDeviceDataService
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<UserDevice> UpsertUserDeviceAsync(UserDevice device)
        {
            
            var existingDevice = await _dbContext.Set<UserDevice>()
                .FirstOrDefaultAsync(x => x.UserId == device.UserId && x.DeviceId == device.DeviceId);

            if (existingDevice != null)
            {
                // 1. تحديث (Update): الجهاز موجود، نحدث البيانات المتغيرة فقط
                if (!string.IsNullOrEmpty(device.NotifyToken)) existingDevice.NotifyToken = device.NotifyToken;
                if (!string.IsNullOrEmpty(device.DeviceToken)) existingDevice.DeviceToken = device.DeviceToken;
                if (!string.IsNullOrEmpty(device.AppVersion)) existingDevice.AppVersion = device.AppVersion;
                if (!string.IsNullOrEmpty(device.OsVersion)) existingDevice.OsVersion = device.OsVersion;
                if (!string.IsNullOrEmpty(device.DeviceModel)) existingDevice.DeviceModel = device.DeviceModel;
                if (!string.IsNullOrEmpty(device.Platform)) existingDevice.Platform = device.Platform;

                _dbContext.Set<UserDevice>().Update(existingDevice);
            }
            else
            {
                device.NID = Guid.NewGuid();
                await _dbContext.Set<UserDevice>().AddAsync(device);
            }

            await _dbContext.SaveChangesAsync();
            return device;
        }
        public async Task<bool> Delete(Guid id)
        {
            UserDevice? entity = await _dbContext.Set<UserDevice>().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            _dbContext.Set<UserDevice>().Remove(entity!);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<UserDevice> Get(Guid id)
        {
            UserDevice? entity = await _dbContext.Set<UserDevice>().AsNoTracking().FirstOrDefaultAsync((e) => e.NID == id);
            if (entity == null)
                throw new Exception();
            return entity!;
        }
        public async Task<IEnumerable<UserDevice>> GetAllByUser(string? userid)
        {
            IEnumerable<UserDevice>? entities = await _dbContext.Set<UserDevice>().Where(x => x.UserId == userid && x.IsActive).ToListAsync();
            return entities;
        }

        public async Task<UserDevice> GetUserDevice(string? userid, string? Deviceid)
        {
            UserDevice? entity = await _dbContext.Set<UserDevice>().FirstOrDefaultAsync(x => x.UserId == userid && x.DeviceId == Deviceid);
            if (entity != null)
            {
                return entity;

            }
            return new();
        }

    }
}
