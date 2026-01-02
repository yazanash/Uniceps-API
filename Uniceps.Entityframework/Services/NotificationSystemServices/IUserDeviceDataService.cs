using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.NotificationModels;

namespace Uniceps.Entityframework.Services.NotificationSystemServices
{
    public interface IUserDeviceDataService
    {
        Task<UserDevice> UpsertUserDeviceAsync(UserDevice entity);
        Task<bool> Delete(Guid id);
        Task<UserDevice> Get(Guid id);
        Task<IEnumerable<UserDevice>> GetAllByUser(string? userid);
        Task<UserDevice> GetUserDevice(string? userid, string? Deviceid);
    }
}
