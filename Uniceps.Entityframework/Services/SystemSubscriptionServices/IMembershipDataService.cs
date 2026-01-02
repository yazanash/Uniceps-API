using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public interface IMembershipDataService
    {
        Task<SystemSubscription> Create(SystemSubscription entity);
        Task<bool> Delete(Guid id);
        Task<SystemSubscription> Get(Guid id);
        Task<SystemSubscription> GetActiveSubscriptionByAppId(string userid, int productId);
        Task<IEnumerable<SystemSubscription>> GetByUserIdListAsync(string userid);
        Task<SystemSubscription> Update(SystemSubscription entity);
        Task<bool> HasUsedTrialForProduct(string userID,int productId);
    }
}
