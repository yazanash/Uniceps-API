using Uniceps.Entityframework.Models.AuthenticationModels;
using Uniceps.Entityframework.Models.SystemSubscriptionModels;

namespace Uniceps.app.Services.PaymentServices
{
    public interface IPaymentGateway
    {
        Task<string?> CreateSessionAsync(SystemSubscription sub, AppUser user, PlanModel plan);
    }
}
