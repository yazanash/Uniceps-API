using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Core.Services;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public interface ICouponDataService:IIntDataService<SubscriptionCoupon>
    {
        Task<SubscriptionCoupon> GetByCodeAsync(string code);
    }
}
