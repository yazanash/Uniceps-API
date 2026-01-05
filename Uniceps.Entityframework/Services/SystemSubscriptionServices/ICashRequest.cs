using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.Entityframework.Services.SystemSubscriptionServices
{
    public interface ICashRequest
    {
        Task<CashPaymentRequest> Create(CashPaymentRequest entity);
        Task<bool> Delete(int id);
        Task<CashPaymentRequest> Get(int id);
        Task<IEnumerable<CashPaymentRequest>> GetAll(CashRequestStatus status);
        Task<CashPaymentRequest> Update(CashPaymentRequest entity);
    }
}
