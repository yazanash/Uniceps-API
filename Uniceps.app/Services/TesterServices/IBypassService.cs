using Uniceps.app.DTOs.SystemSubscriptionDtos;

namespace Uniceps.app.Services.TesterServices
{
    public interface IBypassService
    {
        bool IsValidTester(string email, string otp);
        bool IsTester(string email);
        SystemSubscriptionDto? GetSubscriptionForTester();
    }
}
